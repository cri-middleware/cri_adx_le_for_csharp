/****************************************************************************
 * CRI Middleware SDK
 * 
 * Copyright (c) 2016 CRI Middleware Co., Ltd.
 * 
 * Library  : CRI File System
 * Module   : I/O interface of JavaScript library
 * File     : crifs_io.jslib
 ****************************************************************************/

var LibraryCriFsIo = {
    /*==========================================================================
     * Main File I/O Manager
     * Handles file operations for both browser and WeChat environments
     *========================================================================*/
    $CriFsIo: {
        // File tracking
        fileList: {},
        
        // WeChat file system manager
        fsManager: null,
        
        // File status constants
        STATUS: {
            INITIALIZED: "initialized",
            LOADING: "loading",
            COMPLETED: "completed"
        },
        
        /*----------------------------------------------------------------------
         * Browser-specific file fetching
         *--------------------------------------------------------------------*/
        /**
         * Fetch file from URL (browser only)
         * @param {string} filePath - URL of the file
         * @param {boolean} head - Whether to perform HEAD request only
         * @param {{offset: number, size: number}|null} range - Byte range for partial content
         * @returns {Promise<{fileSize: number, eTag: string|null, data: ArrayBuffer|null}>}
         */
        fetchFile: async function(filePath, head = false, range = null) {
            // Skip if in WeChat environment
            if (typeof wx !== 'undefined') return null;
            
            try {
                const options = head ? { method: 'HEAD' } : {};
                
                // Add range header if specified
                if (range && typeof range.offset === 'number' && typeof range.size === 'number') {
                    const start = range.offset;
                    const end = range.offset + range.size - 1;
                    if (!options.headers) {
                        options.headers = {};
                    }
                    options.headers['Range'] = `bytes=${start}-${end}`;
                }
                
                const response = await fetch(filePath, options);
                
                // Check response status
                if (!response.ok && response.status !== 206 && !head) {
                    throw new Error(`Failed to fetch ${filePath}. Status: ${response.status}`);
                }
                
                const fileSize = parseInt(response.headers.get("Content-Length") || "-1");
                const eTag = response.headers.get("ETag");
                const data = head ? null : await response.arrayBuffer();
                
                return { fileSize, eTag, data };
            } catch (error) {
                console.error(`Error fetching ${filePath}:`, error);
                return { fileSize: -1, eTag: null, data: null };
            }
        },
        
        /*----------------------------------------------------------------------
         * WeChat-specific helpers
         *--------------------------------------------------------------------*/
        /**
         * Convert URL to relative path for WeChat file system
         * @param {string} url - Full URL
         * @param {string} subpathToRemove - Path prefix to remove
         * @returns {string|null} Relative path or null
         */
        getRelativePath: function(url, subpathToRemove) {
            // Skip if not in WeChat environment
            if (typeof wx === 'undefined') return null;
            
            try {
                const parsedUrl = new URL(url);
                let relativePath = parsedUrl.pathname.replace(/^\/+/, '');
                
                // Remove specified subpath if present
                if (relativePath.startsWith(subpathToRemove)) {
                    relativePath = relativePath.substring(subpathToRemove.length);
                }
                
                return relativePath.replace(/^\/+/, '');
            } catch (e) {
                console.error("Invalid URL:", e);
                return null;
            }
        },
        
        /**
         * Initialize file info in fileList
         * @param {string} filePath - Path of the file
         * @returns {Object} File info object
         */
        initFileInfo: function(filePath) {
            if (!(filePath in this.fileList)) {
                this.fileList[filePath] = {
                    fileSize: -1,
                    data: null,
                    status: this.STATUS.INITIALIZED,
                    handleList: {},
                    eTag: null
                };
            }
            return this.fileList[filePath];
        },
        
        /*----------------------------------------------------------------------
         * WeChat file loading implementation
         *--------------------------------------------------------------------*/
        /**
         * Load file in WeChat environment
         * @param {string} filePath - Path of the file
         * @param {Object} fileInfo - File info object
         */
        loadWeChat: function(filePath, fileInfo) {
            // Initialize file system manager if needed
            if (this.fsManager === null) {
                this.fsManager = wx.getFileSystemManager();
            }
            
            // Skip if already loading
            if (fileInfo.status === this.STATUS.LOADING) {
                return;
            }
            
            // Skip if already loaded
            if (fileInfo.fileSize !== -1) {
                return;
            }
            
            fileInfo.status = this.STATUS.LOADING;
            
            const isLocalhost = filePath.includes('localhost') || filePath.includes('127.0.0.1');
            
            if (isLocalhost) {
                // Handle local file
                this.loadLocalFileWeChat(filePath, fileInfo);
            } else {
                // Handle remote file
                this.loadRemoteFileWeChat(filePath, fileInfo);
            }
        },
        
        /**
         * Load local file in WeChat
         * @param {string} filePath - Path of the file
         * @param {Object} fileInfo - File info object
         */
        loadLocalFileWeChat: function(filePath, fileInfo) {
            const absoluteFilePath = this.getRelativePath(filePath, "game/");
            
            this.fsManager.readFile({
                filePath: absoluteFilePath,
                success: (result) => {
                    fileInfo.fileSize = result.data.byteLength;
                    fileInfo.data = result.data;
                },
                fail: (result) => {
                    console.error(`Error loading local file ${filePath}:`, result);
                    fileInfo.fileSize = -1;
                    fileInfo.data = null;
                },
                complete: () => {
                    fileInfo.status = this.STATUS.COMPLETED;
                }
            });
        },
        
        /**
         * Load remote file in WeChat
         * @param {string} filePath - Path of the file
         * @param {Object} fileInfo - File info object
         */
        loadRemoteFileWeChat: function(filePath, fileInfo) {
            wx.downloadFile({
                url: filePath,
                success: (res) => {
                    if (res.statusCode === 200) {
                        // Read the downloaded file
                        this.fsManager.readFile({
                            filePath: res.tempFilePath,
                            success: (result) => {
                                fileInfo.fileSize = result.data.byteLength;
                                fileInfo.data = result.data;
                            },
                            fail: (result) => {
                                console.error(`Error reading downloaded file ${filePath}:`, result);
                                fileInfo.fileSize = -1;
                                fileInfo.data = null;
                            },
                            complete: () => {
                                fileInfo.status = this.STATUS.COMPLETED;
                            }
                        });
                    } else {
                        console.error(`Download failed for ${filePath}. Status code: ${res.statusCode}`);
                        fileInfo.fileSize = -1;
                        fileInfo.data = null;
                        fileInfo.status = this.STATUS.COMPLETED;
                    }
                },
                fail: (error) => {
                    console.error(`Error downloading file ${filePath}:`, error);
                    fileInfo.fileSize = -1;
                    fileInfo.data = null;
                    fileInfo.status = this.STATUS.COMPLETED;
                }
            });
        },
        
        /*----------------------------------------------------------------------
         * Browser file loading implementation
         *--------------------------------------------------------------------*/
        /**
         * Load file in browser environment
         * @param {string} filePath - Path of the file
         * @param {Object} fileInfo - File info object
         */
        loadBrowser: async function(filePath, fileInfo) {
            // Skip if already loading
            if (fileInfo.status === this.STATUS.LOADING) {
                return;
            }
            
            // Skip if already loaded
            if (fileInfo.fileSize !== -1) {
                return;
            }
            
            try {
                fileInfo.status = this.STATUS.LOADING;
                
                // Fetch file metadata (HEAD request)
                const { fileSize, eTag } = await this.fetchFile(filePath, true);
                
                fileInfo.fileSize = fileSize;
                fileInfo.eTag = eTag;
                
            } catch (error) {
                console.error(`Error loading ${filePath}:`, error);
                fileInfo.fileSize = -1;
            } finally {
                fileInfo.status = this.STATUS.COMPLETED;
            }
        },
        
        /*----------------------------------------------------------------------
         * Public API Methods
         *--------------------------------------------------------------------*/
        /**
         * Load a file (environment-aware)
         * @param {string} filePath - Path of the file to load
         */
        load: function(filePath) {
            const fileInfo = this.initFileInfo(filePath);
            
            if (typeof wx !== 'undefined') {
                // WeChat environment
                this.loadWeChat(filePath, fileInfo);
            } else {
                // Browser environment
                this.loadBrowser(filePath, fileInfo);
            }
        },
        
        /**
         * Check if file is fully loaded
         * @param {string} filePath - Path of the file
         * @returns {boolean} true if loaded
         */
        isLoaded: function(filePath) {
            if (!(filePath in this.fileList)) {
                return false;
            }
            const fileInfo = this.fileList[filePath];
            return fileInfo.status === this.STATUS.COMPLETED;
        },
        
        /**
         * Get file size
         * @param {string} filePath - Path of the file
         * @returns {number} File size in bytes or -1 if not available
         */
        getFileSize: function(filePath) {
            if (!(filePath in this.fileList)) {
                return -1;
            }
            const fileInfo = this.fileList[filePath];
            return fileInfo.fileSize;
        },
        
        /**
         * Read data from file
         * @param {number} handle - Request handle
         * @param {string} filePath - Path of the file
         * @param {number} offset - Byte offset to start reading
         * @param {number} length - Number of bytes to read
         * @param {number} pointer - Memory pointer to write data
         */
        read: function(handle, filePath, offset, length, pointer) {
            if (!(filePath in this.fileList)) {
                throw new Error(`File ${filePath} not found in fileList`);
            }
            
            const fileInfo = this.fileList[filePath];
            
            // Initialize or get request handle
            let requestHandle = fileInfo.handleList[handle];
            if (!requestHandle) {
                requestHandle = {
                    readSize: 0,
                    isCompleted: false
                };
                fileInfo.handleList[handle] = requestHandle;
            }
            
            // Reset handle state
            requestHandle.readSize = 0;
            requestHandle.isCompleted = false;
            
            if (typeof wx !== 'undefined') {
                // WeChat: Read from memory
                this.readFromMemory(fileInfo.data, offset, length, pointer, requestHandle);
            } else {
                // Browser: Read asynchronously
                this.readAsync(filePath, fileInfo, offset, length, pointer, requestHandle);
            }
        },
        
        /**
         * Read data from memory buffer
         * @param {ArrayBuffer} data - Source data
         * @param {number} offset - Byte offset
         * @param {number} length - Bytes to read
         * @param {number} pointer - Destination pointer
         * @param {Object} requestHandle - Request handle object
         */
        readFromMemory: function(data, offset, length, pointer, requestHandle) {
            if (!data) {
                requestHandle.readSize = -1;
                requestHandle.isCompleted = true;
                return;
            }
            
            requestHandle.readSize = Math.min(length, data.byteLength - offset);
            const responseArray = new Uint8Array(data.slice(offset, offset + requestHandle.readSize));
            const buffer = new Uint8Array(Module['HEAPU8'].buffer, pointer, requestHandle.readSize);
            buffer.set(responseArray);
            requestHandle.isCompleted = true;
        },
        
        /**
         * Read data asynchronously in browser
         * @param {string} filePath - Path of the file
         * @param {Object} fileInfo - File info object
         * @param {number} offset - Byte offset
         * @param {number} length - Bytes to read
         * @param {number} pointer - Destination pointer
         * @param {Object} requestHandle - Request handle object
         */
        readAsync: async function(filePath, fileInfo, offset, length, pointer, requestHandle) {
            try {
                // Fetch data with range request
                const response = await this.fetchFile(filePath, false, { offset, size: length });
                
                if (response.data) {
                    requestHandle.readSize = Math.min(length, response.data.byteLength);
                    const responseArray = new Uint8Array(response.data, 0, requestHandle.readSize);
                    const buffer = new Uint8Array(Module['HEAPU8'].buffer, pointer, requestHandle.readSize);
                    buffer.set(responseArray);
                } else {
                    requestHandle.readSize = -1;
                }
                
            } catch (error) {
                console.error(`Error reading ${filePath}:`, error);
                requestHandle.readSize = -1;
            } finally {
                requestHandle.isCompleted = true;
            }
        },
        
        /**
         * Check if read operation is completed
         * @param {number} handle - Request handle
         * @param {string} filePath - Path of the file
         * @returns {boolean} true if completed
         */
        isReadCompleted: function(handle, filePath) {
            if (!(filePath in this.fileList)) {
                return false;
            }
            
            const fileInfo = this.fileList[filePath];
            const requestHandle = fileInfo.handleList[handle];
            
            return requestHandle ? requestHandle.isCompleted : false;
        },
        
        /**
         * Get number of bytes read
         * @param {number} handle - Request handle
         * @param {string} filePath - Path of the file
         * @returns {number} Bytes read or -1 on error
         */
        getReadSize: function(handle, filePath) {
            if (!(filePath in this.fileList)) {
                return -1;
            }
            
            const fileInfo = this.fileList[filePath];
            const requestHandle = fileInfo.handleList[handle];
            
            return requestHandle ? requestHandle.readSize : -1;
        },
        
        /**
         * Clean up resources for a handle
         * @param {number} handle - Request handle
         * @param {string} filePath - Path of the file
         */
        unload: function(handle, filePath) {
            if (!(filePath in this.fileList)) {
                return;
            }
            
            const fileInfo = this.fileList[filePath];
            delete fileInfo.handleList[handle];
        }
    },
    
    /*==========================================================================
     * External C/C++ Interface Functions
     * These are called from C/C++ code via Emscripten
     *========================================================================*/
    criFsIoJs_Load: function(path) {
        CriFsIo.load(UTF8ToString(path));
    },
    
    criFsIoJs_IsLoaded: function(path) {
        return CriFsIo.isLoaded(UTF8ToString(path));
    },
    
    criFsIoJs_GetFileSize: function(path) {
        return CriFsIo.getFileSize(UTF8ToString(path));
    },
    
    criFsIoJs_Unload: function(handle, path) {
        CriFsIo.unload(handle, UTF8ToString(path));
    },
    
    criFsIoJs_Read: function(handle, path, offset, length, pointer) {
        CriFsIo.read(handle, UTF8ToString(path), offset, length, pointer);
    },
    
    criFsIoJs_IsReadCompleted: function(handle, path) {
        return CriFsIo.isReadCompleted(handle, UTF8ToString(path));
    },
    
    criFsIoJs_GetReadSize: function(handle, path) {
        return CriFsIo.getReadSize(handle, UTF8ToString(path));
    }
};

// Register dependencies for Emscripten
autoAddDeps(LibraryCriFsIo, '$CriFsIo');
mergeInto(LibraryManager.library, LibraryCriFsIo);

/* --- end of file --- */