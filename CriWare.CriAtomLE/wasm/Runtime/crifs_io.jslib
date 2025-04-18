/****************************************************************************
 *
 * CRI Middleware SDK
 *
 * Copyright (c) 2016 CRI Middleware Co., Ltd.
 *
 * Library	: CRI File System
 * Module	 : I/O interface of JavaScript library
 * File		 : crifs_io.jslib
 *
 ****************************************************************************/
var LibraryCriFsIo = {
	/*==========================================================================
	/* 内部関数
	/*========================================================================*/
	$CriFsIDB:{
		indexedDB: null,
		init: function(){
			return new Promise((resolve) => {
				if (!window.indexedDB) {
					console.warn("IndexedDB is not available in this browser.");
					resolve(false);
					return;
				}
		
				const request = window.indexedDB.open("CriCache", 1);
		
				request.onupgradeneeded = (event) => {
					const db = event.target.result;
					db.createObjectStore("responses", { keyPath: "url" });
				};
		
				request.onsuccess = (event) => {
					CriFsIDB.indexedDB = event.target.result;
					resolve(true);
				};
		
				request.onerror = (event) => {
					console.error("IndexedDB initialization error:", event.target.error);
					CriFsIDB.indexedDB = null;
					resolve(false);
				};
			});
		},
		put: function(key, eTag ,data){
			return new Promise((resolve, reject) => {
				if (!CriFsIDB.indexedDB) {
					reject(new Error("IndexedDB is not initialized"));
					return;
				}
			
				const transaction = CriFsIDB.indexedDB.transaction(["responses"], "readwrite");
				const objectStore = transaction.objectStore("responses");
				const request = objectStore.put({url: key, eTag: eTag ,data: data});
			
				request.onsuccess = () => resolve(true);
				request.onerror = () => reject(new Error("Failed to store data in IndexedDB"));
			});
			},
		get: function(key){
			return new Promise((resolve, reject) => {
				if (!CriFsIDB.indexedDB) {
					reject(new Error("IndexedDB is not initialized"));
					return;
				}
		
				const transaction = CriFsIDB.indexedDB.transaction(["responses"], "readonly");
				const objectStore = transaction.objectStore("responses");
				const request = objectStore.get(key);
		
				request.onsuccess = (event) => {
					const result = event.target.result;
					resolve(result ? {data:result.data, eTag:result.eTag} : null);
				};
		
				request.onerror = () => reject(new Error("Failed to retrieve data from IndexedDB"));
			});
			},
	},
	$CriFsIo: {
		/*----------------------------------------------------------------------
		/* 基本関数
		/*--------------------------------------------------------------------*/
		/* ファイルの検索 */
		fetchFile: async function(filePath, head = false) {
			try {
				const options = head ? { method: 'HEAD' } : {};
				const response = await fetch(filePath, options);
				
				if (!response.ok) {
					throw new Error(`Failed to fetch ${filePath}. Status: ${response.status}`);
				}
				
				const fileSize = response.headers.get("Content-Length");
				const eTag = response.headers.get("ETag");
				const data = head ? null : await response.arrayBuffer();
				
				return { fileSize, eTag, data };
			} catch (error) {
				return { fileSize: -1, eTag: null, data: null };
			}
		},

		/*----------------------------------------------------------------------
		/* CriFsIoインターフェース
		/*--------------------------------------------------------------------*/
		/* ロード済みファイルリスト */
		fileList: {},
		isIDBAvailable: true,
		
		/* ファイルのロード */
		load: async function (filePath) {
			// IDBが未初期化の場合初期化
			if (CriFsIo.isIDBAvailable && CriFsIDB.indexedDB === null) {
				CriFsIo.isIDBAvailable = await new Promise(resolve => CriFsIDB.init(resolve));
			}
			
			if (!(filePath in CriFsIo.fileList)) {
				CriFsIo.fileList[filePath] = {
					fileSize: -1,
					isCached: false,
					status: "initialized",
					handleList: {}
				};
			}
			
			const fileInfo = CriFsIo.fileList[filePath];

			if(fileInfo.status == "loading"){
				return;
			}
			
			try {
				if (fileInfo.isCached) {
					const { fileSize, eTag: newETag} = await CriFsIo.fetchFile(filePath, true); // キャッシュ最新化のためヘッダーを取得
					const cachedData = await CriFsIDB.get(filePath);
				
					if (cachedData){
						if(newETag && cachedData.eTag !== newETag) {
							const { fileSize: newFileSize, eTag: newETag, data } = await CriFsIo.fetchFile(filePath, false);
							
							if (newFileSize !== -1) {
								const result = await CriFsIDB.put(filePath, data);
								if (result) {
									fileInfo.fileSize = newFileSize;
									fileInfo.eTag = newETag;
								} else {
									console.warn(`W2024072901JS: Failed to rewrite cache data ${filePath}.`);
								}
							} else {
								console.warn(`W2024072900JS: Failed to renew cache data ${filePath}.`);
							}
						} // ヘッダー取得が失敗しても続行する
					} else {
						// キャッシュ読み込みが失敗したときはキャッシュされていないことにする
						fileInfo.isCached = false;
					}
				} else if (CriFsIDB.indexedDB) {
					fileInfo.status = "loading";
					// ファイルをサーバーから取得してキャッシングする
					const { fileSize, eTag, data } = await CriFsIo.fetchFile(filePath, false);
				
					if (fileSize !== -1) {
						const result = await CriFsIDB.put(filePath, eTag, data);
						if (result) {
							fileInfo.fileSize = fileSize;
							fileInfo.eTag = eTag;
							fileInfo.isCached = true;
						} else {
							console.warn(`W2024072901JS: Failed to write cache data ${filePath}.`);
							fileInfo.fileSize = -1;
						}
					} else {
						console.warn(`W2024072902JS: Failed to get data ${filePath}.`);
						fileInfo.fileSize = -1;
					}
				} else if (fileInfo.fileSize === -1) {
					// キャッシングが使えない場合ファイルデータだけ取ってくる
					fileInfo.status = "loading";
					const { fileSize, eTag } = await CriFsIo.fetchFile(filePath, true);
					fileInfo.fileSize = fileSize; // 失敗するとFileSizeが-1になる
					fileInfo.eTag = eTag;
					fileInfo.isCached = false;
				}
			} catch (error) {
				console.error(`Error loading ${filePath}:`, error);
				fileInfo.fileSize = -1;
			} finally {
				// 結果が失敗してもロード終了とする（上位でエラーになる）
				fileInfo.status = "completed";
			}
		},
		isLoaded: function (filePath) {
			/* ファイル情報が取得済みかどうかチェック */
			if (!(filePath in CriFsIo.fileList)) {
				return false;
			}

			/* ファイルがロード済みかどうかチェック */
			var fileInfo = CriFsIo.fileList[filePath];
			return (fileInfo.status == "completed")? true : false;
		},
		/* ファイルサイズの取得 */
		getFileSize: function (filePath) {
			/* ファイル情報が取得済みかどうかチェック */
			if (!(filePath in CriFsIo.fileList)) {
				return -1;
			}

			/* ファイル情報の取得 */
			var fileInfo = CriFsIo.fileList[filePath];

			/* ファイルサイズを返す */
			return fileInfo.fileSize;
		},

		/* ファイルのリード */
		read: async function (handle, filePath, offset, length, pointer) {
			if (!(filePath in CriFsIo.fileList)) {
				throw new Error(`File ${filePath} not found in fileList`);
			}
		
			const fileInfo = CriFsIo.fileList[filePath];
			let requestHandle = fileInfo.handleList[handle];
		
			if (!requestHandle) {
				requestHandle = {};
				// ファイルハンドル１つにつきリードリクエストは1つのみ
				fileInfo.handleList[handle] = requestHandle;
			}
		
			requestHandle.readSize = 0;
			requestHandle.isCompleted = false;
		
			try {
				if (fileInfo.isCached) {
					var cachedData = await CriFsIDB.get(filePath);

					if (cachedData) {
						requestHandle.readSize = Math.min(length, cachedData.data.byteLength - offset);
						const responseArray = new Uint8Array(cachedData.data.slice(offset, offset + requestHandle.readSize));
						const buffer = new Uint8Array(Module['HEAPU8'].buffer, pointer, requestHandle.readSize);
						buffer.set(responseArray);
					} else {
						fileInfo.isCached = false;
						requestHandle.readSize = -1;
					}
				}
				// キャッシュ読み込みが失敗したときもネットワークから処理を続行する
				if (!fileInfo.isCached) {
					const response = await fetch(filePath);
					
					if (!response.ok) {
						throw new Error(`HTTP error! status: ${response.status}`);
					}
			
					const arrayBuffer = await response.arrayBuffer();
					requestHandle.readSize = Math.min(length, arrayBuffer.byteLength - offset);
					const responseArray = new Uint8Array(arrayBuffer.slice(offset, offset + requestHandle.readSize));
					const buffer = new Uint8Array(Module['HEAPU8'].buffer, pointer, requestHandle.readSize);
					buffer.set(responseArray);
				}
			} catch (error) {
				console.error(`Error fetching ${filePath}:`, error);
				requestHandle.readSize = -1;
			} finally {
				requestHandle.isCompleted = true;
			}
		},

		isReadCompleted: function (handle, filePath){
			if (!(filePath in CriFsIo.fileList)) {
				return false;
			}
			var fileInfo = CriFsIo.fileList[filePath];
			if(fileInfo.handleList[handle] == null){
				return false;
			}
			var requestHandle = fileInfo.handleList[handle];
			return requestHandle.isCompleted;
		},

		getReadSize: function(handle, filePath){
			if (!(filePath in CriFsIo.fileList)) {
				return -1;
			}
			var fileInfo = CriFsIo.fileList[filePath];
			if(fileInfo.handleList[handle] == null){
				return -1;
			}
			var requestHandle = fileInfo.handleList[handle];
			return requestHandle.readSize;
		},

		/* ファイルのアンロード */
		unload: function (handle, filePath) {
			/* ファイル情報が取得済みかどうかチェック */
			if (!(filePath in CriFsIo.fileList)) {
				return;
			}

			/* ファイル情報の取得 */
			var fileInfo = CriFsIo.fileList[filePath];

			fileInfo.handleList[handle] = null;
		},
	},

	/*==========================================================================
	/* 外部関数
	/*========================================================================*/
	/* 検索 */
	criFsIoJs_Load: function (path) {
		CriFsIo.load(UTF8ToString(path));
	},

	/* ファイル検索完了チェック */
	criFsIoJs_IsLoaded: function (path) {
		return CriFsIo.isLoaded(UTF8ToString(path));
	},

	/* プリロード済みファイルサイズの取得 */
	criFsIoJs_GetFileSize: function (path) {
		return CriFsIo.getFileSize(UTF8ToString(path));
	},

	/* アンロード */
	criFsIoJs_Unload: function (handle, path) {
		CriFsIo.unload(handle, UTF8ToString(path));
	},

	/* データの読み込み */
	criFsIoJs_Read: function (handle, path, offset, length, pointer) {
		/* コピー元の取得 */
		CriFsIo.read(handle, UTF8ToString(path), offset, length, pointer);
	},

	criFsIoJs_IsReadCompleted:function (handle, path){
		return CriFsIo.isReadCompleted(handle, UTF8ToString(path));
	},

	criFsIoJs_GetReadSize:function (handle, path){
		return CriFsIo.getReadSize(handle, UTF8ToString(path));
	},
};

/* インターフェースの登録 */
autoAddDeps(LibraryCriFsIo, '$CriFsIDB');
autoAddDeps(LibraryCriFsIo, '$CriFsIo');
mergeInto(LibraryManager.library, LibraryCriFsIo);

/* --- end of file --- */
