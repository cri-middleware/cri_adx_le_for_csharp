LibraryCriNcAsr = {
    $CriNcAsr: {
        wactx: null,
        ncvoices: [],
        processors: [],
        interval: null,
        initPromise: null,
        createPromise: null,
        setRenderingRatioFunc: null,
        mainFunc: null,
        useWorklet: false,
        serverFrequency: 60,

        // Enhanced Timer System
        timing: {
            // Core timing state
            targetInterval: 16.67,     // Will be calculated from serverFrequency
            lastExecutionTime: 0,
            nextScheduledTime: 0,
            adaptiveInterval: 16.67,
            consecutiveLateCount: 0,
            isRunning: false,
            
            // Performance tracking
            executionHistory: [],
            historyIndex: 0,
            historySize: 60,
            
            // Drift compensation
            driftCompensation: 0,
            maxDriftCompensation: 2.0,
            
            // Adaptive parameters
            urgencyThreshold: 0.75,    // When to use high-priority scheduling
            maxConsecutiveLate: 5,     // When to consider system overloaded
            adaptiveStepSize: 0.1,     // How quickly to adjust timing
            
            // Microtask safety
            recentMicrotaskCount: 0,
            lastMicrotaskTime: 0,
            microtaskCooldown: 50,     // ms between microtask bursts
            maxConsecutiveMicrotasks: 3,
            
            // Performance metrics
            totalExecutions: 0,
            totalLateExecutions: 0,
            maxLateness: 0,
            lastFrameTime: 0
        },

        // Initialize the enhanced timer system
        // Update initEnhancedTimer to use merged handler
        initEnhancedTimer: function() {            
            // Calculate target interval from server frequency
            CriNcAsr.timing.targetInterval = 1000 / CriNcAsr.serverFrequency;
            CriNcAsr.timing.adaptiveInterval = CriNcAsr.timing.targetInterval;
            
            // Initialize execution history
            CriNcAsr.timing.executionHistory = new Array(CriNcAsr.timing.historySize);
            for (let i = 0; i < CriNcAsr.timing.historySize; i++) {
                CriNcAsr.timing.executionHistory[i] = {
                    scheduledTime: 0,
                    actualTime: 0,
                    lateness: 0,
                    executionDuration: 0,
                    priority: 'normal'
                };
            }
            
            // Reset metrics
            CriNcAsr.timing.totalExecutions = 0;
            CriNcAsr.timing.totalLateExecutions = 0;
            CriNcAsr.timing.maxLateness = 0;
            
            // Setup event handlers
            if (typeof document !== 'undefined') {
                document.addEventListener('visibilitychange', CriNcAsr.handleVisibilityChange);
            }
            if (typeof window !== 'undefined') {
                window.addEventListener('focus', CriNcAsr.handleFocusChange);
                window.addEventListener('blur', CriNcAsr.handleFocusChange);
            }
            
        },

        // Start the enhanced timer
        startEnhancedTimer: function() {
            if (CriNcAsr.timing.isRunning) {
                console.warn("CriNcAsr: Timer already running");
                return;
            }
            
            CriNcAsr.timing.isRunning = true;
            CriNcAsr.timing.lastExecutionTime = performance.now();
            CriNcAsr.timing.nextScheduledTime = CriNcAsr.timing.lastExecutionTime + CriNcAsr.timing.targetInterval;
            CriNcAsr.timing.lastFrameTime = CriNcAsr.timing.lastExecutionTime;
            
            console.log("CriNcAsr: Starting enhanced timer");
            CriNcAsr.scheduleNextExecution();
        },

        // Stop the enhanced timer
        stopEnhancedTimer: function() {
            CriNcAsr.timing.isRunning = false;
            console.log("CriNcAsr: Stopping enhanced timer");
        },

        // Schedule the next execution with adaptive priority
        scheduleNextExecution: function() {
            if (!CriNcAsr.timing.isRunning || !CriNcAsr.mainFunc) {
                return;
            }
            
            const now = performance.now();
            const timeUntilNext = CriNcAsr.timing.nextScheduledTime - now;
            const urgency = 1 - (timeUntilNext / CriNcAsr.timing.targetInterval);
            
            let priority = 'normal';
            
            // Determine scheduling priority based on multiple factors
            if (timeUntilNext < -CriNcAsr.timing.targetInterval) {
                // We're more than one interval late
                priority = 'critical';
            } else if (timeUntilNext < 0) {
                // We're late
                priority = 'immediate';
            } else if (urgency > CriNcAsr.timing.urgencyThreshold) {
                // We're approaching our deadline
                priority = 'high';
            } else if (CriNcAsr.timing.consecutiveLateCount > 2) {
                // We've been late recently, increase priority
                priority = 'elevated';
            }
            
            // Check if we should use microtasks (with safety limits)
            const timeSinceLastMicrotask = now - CriNcAsr.timing.lastMicrotaskTime;
            const canUseMicrotask = 
                CriNcAsr.timing.recentMicrotaskCount < CriNcAsr.timing.maxConsecutiveMicrotasks &&
                timeSinceLastMicrotask > CriNcAsr.timing.microtaskCooldown;
            
            // Schedule based on priority
            switch (priority) {
                case 'critical':
                    if (canUseMicrotask) {
                        CriNcAsr.timing.recentMicrotaskCount++;
                        CriNcAsr.timing.lastMicrotaskTime = now;
                        queueMicrotask(() => {
                            CriNcAsr.executeMainFunction('microtask');
                            // Reset microtask count after a delay
                            setTimeout(() => {
                                CriNcAsr.timing.recentMicrotaskCount = 0;
                            }, 100);
                        });
                    } else {
                        // Fall back to Promise if we can't use microtask
                        Promise.resolve().then(() => CriNcAsr.executeMainFunction('promise'));
                    }
                    break;
                    
                case 'immediate':
                case 'high':
                    Promise.resolve().then(() => CriNcAsr.executeMainFunction('promise'));
                    break;
                    
                case 'elevated':
                    setTimeout(() => CriNcAsr.executeMainFunction('timeout-0'), 0);
                    break;
                    
                case 'normal':
                default:
                    const delay = Math.max(0, timeUntilNext);
                    setTimeout(() => CriNcAsr.executeMainFunction('timeout'), delay);
                    break;
            }
        },

        // Execute the main function and handle timing
        executeMainFunction: function(schedulingMethod) {
            if (!CriNcAsr.timing.isRunning || !CriNcAsr.mainFunc) {
                return;
            }
            
            const executionStartTime = performance.now();
            const scheduledTime = CriNcAsr.timing.nextScheduledTime;
            const lateness = executionStartTime - scheduledTime;
            const frameTime = executionStartTime - CriNcAsr.timing.lastFrameTime;
            
            // Update frame time tracking
            CriNcAsr.timing.lastFrameTime = executionStartTime;
            
            // Execute the actual main function
            let executionSuccess = true;
            try {
                dynCall("v", CriNcAsr.mainFunc, []);
            } catch (error) {
                console.error("CriNcAsr: Error in main function execution:", error);
                executionSuccess = false;
            }
            
            // Measure execution duration
            const executionDuration = performance.now() - executionStartTime;
            
            // Update execution history
            const historyEntry = {
                scheduledTime: scheduledTime,
                actualTime: executionStartTime,
                lateness: lateness,
                executionDuration: executionDuration,
                priority: schedulingMethod,
                frameTime: frameTime,
                success: executionSuccess
            };
            
            CriNcAsr.timing.executionHistory[CriNcAsr.timing.historyIndex] = historyEntry;
            CriNcAsr.timing.historyIndex = (CriNcAsr.timing.historyIndex + 1) % CriNcAsr.timing.historySize;
            
            // Update metrics
            CriNcAsr.timing.totalExecutions++;
            if (lateness > 2) {
                CriNcAsr.timing.totalLateExecutions++;
                CriNcAsr.timing.consecutiveLateCount++;
            } else {
                CriNcAsr.timing.consecutiveLateCount = 0;
            }
            
            if (lateness > CriNcAsr.timing.maxLateness) {
                CriNcAsr.timing.maxLateness = lateness;
            }
            
            // Adapt timing based on performance
            CriNcAsr.adaptTiming();
            
            // Update scheduling state
            CriNcAsr.timing.lastExecutionTime = executionStartTime;
            CriNcAsr.timing.nextScheduledTime += CriNcAsr.timing.adaptiveInterval;
            
            // Handle frame skipping if we're very late
            if (CriNcAsr.timing.nextScheduledTime < executionStartTime) {
                const skip = Math.ceil((executionStartTime - CriNcAsr.timing.nextScheduledTime) / 
                                       CriNcAsr.timing.targetInterval);
                CriNcAsr.timing.nextScheduledTime += skip * CriNcAsr.timing.targetInterval;
                
                if (skip > 1) {
                    console.warn(`CriNcAsr: Skipped ${skip - 1} execution(s) due to timing delay`);
                }
            }
            
            // Schedule next execution
            CriNcAsr.scheduleNextExecution();
        },

        // Adapt timing based on recent performance
        adaptTiming: function() {
            const recentHistory = CriNcAsr.getRecentHistory(10);
            if (recentHistory.length < 5) {
                return; // Not enough data to adapt
            }
            
            // Calculate performance metrics
            let totalLateness = 0;
            let totalDuration = 0;
            let lateCount = 0;
            let maxLateness = 0;
            
            for (const entry of recentHistory) {
                totalLateness += entry.lateness;
                totalDuration += entry.executionDuration;
                if (entry.lateness > 2) {
                    lateCount++;
                }
                if (entry.lateness > maxLateness) {
                    maxLateness = entry.lateness;
                }
            }
            
            const avgLateness = totalLateness / recentHistory.length;
            const avgDuration = totalDuration / recentHistory.length;
            const latePercentage = lateCount / recentHistory.length;
            
            // Drift compensation
            if (Math.abs(avgLateness) > 0.5) {
                const compensation = Math.max(
                    -CriNcAsr.timing.maxDriftCompensation,
                    Math.min(CriNcAsr.timing.maxDriftCompensation, -avgLateness * 0.1)
                );
                CriNcAsr.timing.driftCompensation += compensation;
            } else {
                // Gradually reduce compensation when timing is stable
                CriNcAsr.timing.driftCompensation *= 0.95;
            }
            
            // Adaptive interval adjustment
            let intervalAdjustment = 0;
            
            if (latePercentage > 0.7) {
                // We're late too often
                intervalAdjustment = 0.5;
                console.warn(`CriNcAsr: High lateness rate (${(latePercentage * 100).toFixed(1)}%)`);
            } else if (latePercentage > 0.5) {
                // Moderate lateness
                intervalAdjustment = 0.2;
            } else if (avgLateness < -3 && latePercentage < 0.1) {
                // We're consistently early
                intervalAdjustment = -0.5;
            }
            
            // Apply adjustments
            CriNcAsr.timing.adaptiveInterval = CriNcAsr.timing.targetInterval + 
                                          CriNcAsr.timing.driftCompensation + 
                                          intervalAdjustment * CriNcAsr.timing.adaptiveStepSize;
            
            // Constrain adaptive interval to reasonable bounds
            const minInterval = CriNcAsr.timing.targetInterval * 0.9;
            const maxInterval = CriNcAsr.timing.targetInterval * 1.1;
            CriNcAsr.timing.adaptiveInterval = Math.max(minInterval, 
                                                   Math.min(maxInterval, 
                                                          CriNcAsr.timing.adaptiveInterval));
        },

        // Get recent execution history
        getRecentHistory: function(count) {
            const history = [];
            for (let i = 0; i < Math.min(count, CriNcAsr.timing.historySize); i++) {
                const index = (CriNcAsr.timing.historyIndex - 1 - i + CriNcAsr.timing.historySize) % 
                             CriNcAsr.timing.historySize;
                const entry = CriNcAsr.timing.executionHistory[index];
                if (entry && entry.actualTime > 0) {
                    history.push(entry);
                }
            }
            return history;
        },

        handleVisibilityChange: function() {
            if (document.hidden) {
                // From timing system: reset adaptive timing
                CriNcAsr.timing.adaptiveInterval = CriNcAsr.timing.targetInterval;
                CriNcAsr.timing.consecutiveLateCount = 0;
                
                // From audio system: suspend audio
                if (CriNcAsr.wactx) {
                    CriNcAsr.wactx.suspend();
                }
            } else {
                // From timing system: reset timing to prevent catch-up burst
                CriNcAsr.timing.lastExecutionTime = performance.now();
                CriNcAsr.timing.nextScheduledTime = CriNcAsr.timing.lastExecutionTime + CriNcAsr.timing.targetInterval;
                
                // From audio system: resume audio with delay
                if (CriNcAsr.wactx) {
                    setTimeout(() => {
                        CriNcAsr.wactx.suspend();
                        CriNcAsr.wactx.resume();
                    }, 200);
                }
            }
        },
        
        // Add new focus change handler (same behavior as visibility)
        handleFocusChange: function() {
            if (!document.hasFocus()) {
                // Page lost focus - same behavior as visibility
                CriNcAsr.timing.adaptiveInterval = CriNcAsr.timing.targetInterval;
                CriNcAsr.timing.consecutiveLateCount = 0;
                
                // Suspend audio when focus is lost
                if (CriNcAsr.wactx) {
                    CriNcAsr.wactx.suspend();
                }
            } else {
                // Page gained focus - reset timing and resume audio
                CriNcAsr.timing.lastExecutionTime = performance.now();
                CriNcAsr.timing.nextScheduledTime = CriNcAsr.timing.lastExecutionTime + CriNcAsr.timing.targetInterval;
                
                // Resume audio when focus is gained
                if (CriNcAsr.wactx) {
                    setTimeout(() => {
                        CriNcAsr.wactx.suspend();
                        CriNcAsr.wactx.resume();
                    }, 200);
                }
            }
        },

        // Get timing statistics
        getTimingStats: function() {
            const recentHistory = CriNcAsr.getRecentHistory(30);
            if (recentHistory.length === 0) {
                return {
                    running: false,
                    avgLatency: 0,
                    latePercentage: 0,
                    maxLateness: 0
                };
            }
            
            const avgLatency = recentHistory.reduce((sum, h) => sum + h.lateness, 0) / recentHistory.length;
            const lateCount = recentHistory.filter(h => h.lateness > 2).length;
            const latePercentage = (lateCount / recentHistory.length) * 100;
            
            // Method distribution
            const methodCounts = {};
            recentHistory.forEach(h => {
                methodCounts[h.priority] = (methodCounts[h.priority] || 0) + 1;
            });
            
            return {
                running: CriNcAsr.timing.isRunning,
                serverFrequency: CriNcAsr.serverFrequency,
                targetInterval: CriNcAsr.timing.targetInterval,
                adaptiveInterval: CriNcAsr.timing.adaptiveInterval,
                avgLatency: avgLatency,
                latePercentage: latePercentage,
                maxLateness: CriNcAsr.timing.maxLateness,
                totalExecutions: CriNcAsr.timing.totalExecutions,
                totalLateExecutions: CriNcAsr.timing.totalLateExecutions,
                consecutiveLateCount: CriNcAsr.timing.consecutiveLateCount,
                driftCompensation: CriNcAsr.timing.driftCompensation,
                methodDistribution: methodCounts
            };
        },

        // Existing methods remain unchanged...
        createAudioWorklet: async function(id, num_channels) {
            try {
                await CriNcAsr.initPromise;
                
                var audioWorklet = new AudioWorkletNode(CriNcAsr.wactx, 
                    "cri-ncvoice-audio-worklet-processor", 
                    {outputChannelCount:[num_channels]}
                );
                audioWorklet.bufferStatus = "Empty";
                CriNcAsr.processors[id] = audioWorklet;
            } catch (error) {
                console.error("Failed to create AudioWorklet:", error);
            }
        },

        createScriptProcessor: function(id, num_channels) {
            var processor = CriNcAsr.wactx.createScriptProcessor(512, num_channels, num_channels);
            processor.ringBuffer = [];
            processor.totalStoredSamples = 0;
            processor.offset = 0;
            processor.ncvoice = null;
            
            // Add adaptive buffering properties (matching AudioWorklet)
            processor.targetBufferSize = 2048;        // Start small for low latency
            processor.maxBufferSize = 12000;          // Maximum allowed buffer
            processor.minBufferSize = 2048;           // Minimum buffer size
            
            // Threshold levels (as percentages of target buffer)
            processor.stopRequestThreshold = 0.85;    // Stop requesting at 85% full
            processor.resumeRequestThreshold = 0.50;  // Resume requesting at 50% full
            processor.criticalThreshold = 0.20;       // Critical at 20% full
            processor.emergencyThreshold = 0.10;      // Emergency at 10% full
            
            // State tracking
            processor.isRequestingData = true;
            processor.consecutiveCriticalCounts = 0;
            processor.stableFrameCount = 0;           // Frames since last critical event
            processor.bufferStatus = "Empty";
            
            processor.onaudioprocess = function(e) {
                const output = e.outputBuffer;
                const input = e.inputBuffer;
                var outputOffset = 0;
                
                // Calculate buffer fill ratio
                const bufferFillRatio = this.totalStoredSamples / this.targetBufferSize;
                
                // 1. Handle critical/emergency situations first
                if (bufferFillRatio < this.emergencyThreshold) {
                    this.handleEmergencyState();
                } else if (bufferFillRatio < this.criticalThreshold) {
                    this.handleCriticalState();
                } else {
                    // Reset critical counter when not in critical state
                    this.consecutiveCriticalCounts = 0;
                    this.stableFrameCount++;
                }
                
                // 2. Normal data request management (with hysteresis)
                if (!this.isRequestingData && bufferFillRatio < this.resumeRequestThreshold) {
                    this.isRequestingData = true;
                    this.bufferStatus = "Empty";
                } else if (this.isRequestingData && bufferFillRatio > this.stopRequestThreshold) {
                    this.isRequestingData = false;
                    this.bufferStatus = "Full";
                    if (CriNcAsr.setRenderingRatioFunc) {
                        dynCall("vi", CriNcAsr.setRenderingRatioFunc, [105]);
                    }
                }
                
                // 3. Adaptive buffer size management
                this.adaptiveBufferSizing();
                
                // 4. Copy samples to output (existing logic with minor optimization)
                while(outputOffset < output.length) {
                    var remainedOutput = output.length - outputOffset;
                    var buffers = this.ringBuffer.shift();
                    
                    if(buffers == null) {
                        // Fill remainder with silence
                        for (let channel = 0; channel < output.numberOfChannels; channel++) {
                            const outputData = output.getChannelData(channel);
                            for(let samples = outputOffset; samples < output.length; samples++){
                                outputData[samples] = 0;
                            }
                        }
                        break;
                    }
                    
                    var remainedSamples = buffers[0].length - this.offset;
                    var samplesToCopy = Math.min(remainedOutput, remainedSamples);
                    
                    // Copy samples
                    for (let channel = 0; channel < output.numberOfChannels; channel++) {
                        const outputData = output.getChannelData(channel);
                        for(let samples = 0; samples < samplesToCopy; samples++){
                            outputData[outputOffset + samples] = buffers[channel][this.offset + samples];
                        }
                    }
                    this.totalStoredSamples -= samplesToCopy;
                    
                    if(samplesToCopy == remainedSamples){
                        this.offset = 0;
                    } else {
                        this.offset += samplesToCopy;
                        this.ringBuffer.unshift(buffers);
                    }
                    
                    outputOffset += samplesToCopy;
                }
            };
            
            // Add adaptive methods to the processor
            processor.handleEmergencyState = function() {
                this.consecutiveCriticalCounts++;
                this.stableFrameCount = 0;
                
                // Immediately request data at maximum speed
                this.bufferStatus = "Empty";
                if (CriNcAsr.setRenderingRatioFunc) {
                    dynCall("vi", CriNcAsr.setRenderingRatioFunc, [300]);
                }
                
                // Aggressively increase buffer size
                this.targetBufferSize = Math.min(this.targetBufferSize * 1.5, this.maxBufferSize);
            };
            
            processor.handleCriticalState = function() {
                this.consecutiveCriticalCounts++;
                this.stableFrameCount = 0;
                
                // Request high-speed rendering
                this.bufferStatus = "Empty";
                if (CriNcAsr.setRenderingRatioFunc) {
                    dynCall("vi", CriNcAsr.setRenderingRatioFunc, [300]);
                }
                
                // Moderately increase buffer size
                this.targetBufferSize = Math.min(this.targetBufferSize + 1024, this.maxBufferSize);
            };
            
            processor.adaptiveBufferSizing = function() {
                // Gradually shrink buffer back to minimum when stable
                if (this.stableFrameCount > 300) { // ~5 seconds of stability at 60fps
                    if (this.targetBufferSize > this.minBufferSize) {
                        this.targetBufferSize = Math.max(this.targetBufferSize - 128, this.minBufferSize);
                        this.stableFrameCount = 200; // Reset but keep some stability margin
                    }
                }
            };
            
            CriNcAsr.processors[id] = processor;
        }
    },

    WAASRJS_Initialize: function() {
        var itf = CriNcAsr.itf = Module["CriNcAsrItf"] = Module["CriNcAsrItf"] || {};

        const audioWorkletCode = `class CriNcAsrVoiceAudioWorkletProcessor extends AudioWorkletProcessor {
    constructor() {
        super();
        this.port.onmessage = this.handleMessage.bind(this);
        this.ncvoice = null;
        this.totalStoredSamples = 0;
        this.ringBuffer = [];
        this.offset = 0;
        this.destroyFlag = false;
        
        // Dynamic buffer management
        this.targetBufferSize = 2048;        // Start small for low latency
        this.maxBufferSize = 12000;          // Maximum allowed buffer
        this.minBufferSize = 2048;           // Minimum buffer size
        
        // Threshold levels (as percentages of target buffer)
        this.stopRequestThreshold = 0.85;    // Stop requesting at 85% full
        this.resumeRequestThreshold = 0.50;  // Resume requesting at 50% full
        this.criticalThreshold = 0.20;       // Critical at 20% full
        this.emergencyThreshold = 0.10;      // Emergency at 10% full
        
        // State tracking
        this.isRequestingData = true;
        this.consecutiveCriticalCounts = 0;
        this.stableFrameCount = 0;           // Frames since last critical event
    }

    handleMessage(event) {
        if(event.data['type'] == "Init"){
            this.ncvoice = event.data['ncvoice'];
        }
        if(event.data['type'] == "Data"){
            this.ringBuffer.push(event.data['buffers']);
            this.totalStoredSamples += event.data['length'];
        }
        if(event.data['type'] == "Finalize"){
            this.destroyFlag = true;
        }
    }

    process(inputs, outputs, parameters) {
        const output = outputs[0];
        const bufferFillRatio = this.totalStoredSamples / this.targetBufferSize;
        
        // 1. Handle critical/emergency situations first
        if (bufferFillRatio < this.emergencyThreshold) {
            this.handleEmergencyState();
        } else if (bufferFillRatio < this.criticalThreshold) {
            this.handleCriticalState();
        } else {
            // Reset critical counter when not in critical state
            this.consecutiveCriticalCounts = 0;
            this.stableFrameCount++;
        }
        
        // 2. Normal data request management (with hysteresis)
        if (!this.isRequestingData && bufferFillRatio < this.resumeRequestThreshold) {
            this.isRequestingData = true;
            this.port.postMessage({ 'type': "DataRequest", 'ncvoice': this.ncvoice });
        } else if (this.isRequestingData && bufferFillRatio > this.stopRequestThreshold) {
            this.isRequestingData = false;
            this.port.postMessage({ 'type': "StopDataRequest", 'ncvoice': this.ncvoice });
        }
        
        // 3. Adaptive buffer size management
        this.adaptiveBufferSizing();
        
        // 4. Copy samples to output (your existing logic)
        this.copySamplesToOutput(output);
        
        return !this.destroyFlag;
    }
    
    handleEmergencyState() {
        this.consecutiveCriticalCounts++;
        this.stableFrameCount = 0;
        
        // Immediately request data at maximum speed
        this.port.postMessage({ 'type': "CriticalDataRequest", 'ncvoice': this.ncvoice });
        
        // Aggressively increase buffer size
        this.targetBufferSize = Math.min(this.targetBufferSize * 1.5, this.maxBufferSize);
    }
    
    handleCriticalState() {
        this.consecutiveCriticalCounts++;
        this.stableFrameCount = 0;
        
        // Request high-speed rendering
        this.port.postMessage({ 'type': "CriticalDataRequest", 'ncvoice': this.ncvoice });
        
        // Moderately increase buffer size
        this.targetBufferSize = Math.min(this.targetBufferSize + 1024, this.maxBufferSize);
    }
    
    adaptiveBufferSizing() {
        // Gradually shrink buffer back to minimum when stable
        if (this.stableFrameCount > 300) { // ~5 seconds of stability
            if (this.targetBufferSize > this.minBufferSize) {
                this.targetBufferSize = Math.max(this.targetBufferSize - 128, this.minBufferSize);
                this.stableFrameCount = 200; // Reset but keep some stability margin
            }
        }
    }
    
    copySamplesToOutput(output) {
        // Your existing sample copying logic here
        let outputOffset = 0;
        
        while(outputOffset < output[0].length) {
            const buffers = this.ringBuffer.shift();
            if(!buffers) {
                // Fill remainder with silence
                for (let channel = 0; channel < output.length; channel++) {
                    for(let i = outputOffset; i < output[channel].length; i++){
                        output[channel][i] = 0;
                    }
                }
                break;
            }
            
            const remainedOutput = output[0].length - outputOffset;
            const remainedSamples = buffers[0].length - this.offset;
            const samplesToCopy = Math.min(remainedOutput, remainedSamples);
            
            for (let channel = 0; channel < output.length; channel++) {
                for(let i = 0; i < samplesToCopy; i++){
                    output[channel][outputOffset + i] = buffers[channel][this.offset + i];
                }
            }
            
            this.totalStoredSamples -= samplesToCopy;
            
            if(samplesToCopy === remainedSamples) {
                this.offset = 0;
            } else {
                this.offset += samplesToCopy;
                this.ringBuffer.unshift(buffers);
            }
            
            outputOffset += samplesToCopy;
        }
    }
}
registerProcessor('cri-ncvoice-audio-worklet-processor', CriNcAsrVoiceAudioWorkletProcessor);`;
        const isWeChat = (typeof wx !== 'undefined')? true:false;
        var AudioContext = null;
        if (isWeChat) {
            // WeChat ミニゲーム用のWebAudio AudioContext作成関数を指定
            AudioContext = wx.createWebAudioContext();
        } else {
            AudioContext = window.AudioContext || window.webkitAudioContext;
        }
        if (AudioContext) {
            var context;
            if(isWeChat){
                context = AudioContext;
            } else {
                context = CriNc.wactx || itf["audioContext"] || new AudioContext({sampleRate: 48000});
            }
            
            if (context.audioWorklet && !isWeChat) {
                CriNcAsr.useWorklet = true;
                CriNcAsr.initPromise = context.audioWorklet.addModule('data:text/javascript,' + encodeURI(audioWorkletCode));
            } else {
                CriNcAsr.useWorklet = false;
                CriNcAsr.initPromise = Promise.resolve();
            }
            
            if(typeof navigator !== 'undefined' && navigator.audioSession){ 
                navigator.audioSession.type = 'auto'; 
            }
            
            CriNcAsr.wactx = itf["audioContext"] = context;
            var resume = function(){
                if(CriNcAsr.wactx && CriNcAsr.wactx.state != "running"){
                    CriNcAsr.wactx.suspend();
                    CriNcAsr.wactx.resume();
                }
            };
            if(typeof window !== 'undefined'){ 
                window.addEventListener("mousedown", resume);
                window.addEventListener("touchstart", resume);
            }
        }
    },

    WAASRJS_Create: function(num_channels) {
        var audioProcessor = {};
        CriNcAsr.processors.push(audioProcessor);
        var id = CriNcAsr.processors.indexOf(audioProcessor);
        
        if (CriNcAsr.useWorklet) {
            CriNcAsr.createPromise = CriNcAsr.createAudioWorklet(id, num_channels);
        } else {
            CriNcAsr.createScriptProcessor(id, num_channels);
            CriNcAsr.createPromise = Promise.resolve();
        }

        CriNcAsr.wactx.destination.channelCount = Math.min(num_channels, CriNcAsr.wactx.destination.maxChannelCount);
        
        return id;
    },

    WAASRJS_PutData: function(id, dataptr, num_samples) {
        const processor = CriNcAsr.processors[id];
        if (!processor) return;
        
        const buffers = [];
        const num_channels = processor.channelCount;
        
        for (let i = 0; i < num_channels; i++) {
            const bufferptr = Module['HEAPU32'][dataptr / Uint32Array.BYTES_PER_ELEMENT + i];
            const sharedBuffer = new Float32Array(Module['HEAPF32'].buffer, bufferptr, num_samples);
            const buffer = new Float32Array(num_samples);
            buffer.set(sharedBuffer);
            buffers.push(buffer);
        }
        
        if (CriNcAsr.useWorklet) {
            const transferables = buffers.map(buffer => buffer.buffer);
            processor.port.postMessage({ 
                'type': "Data", 
                'buffers': buffers, 
                'length': num_samples 
            }, transferables);
        } else {
            processor.ringBuffer.push(buffers);
            processor.totalStoredSamples += num_samples;
        }
    },

    WAASRJS_Setup: async function(ncv, id, nch) {
        await CriNcAsr.createPromise;

        const processor = CriNcAsr.processors[id];
        if (!processor) return;
        
        CriNcAsr.ncvoices[id] = ncv;
        
        if (CriNcAsr.useWorklet) {
            processor.channelCount = nch;
            processor.port.postMessage({ 'type': "Init", 'ncvoice': ncv });
            processor.port.onmessage = (event) => {
                if(event.data['type'] == "DataRequest"){
                    processor.bufferStatus = "Empty";
                } else if(event.data['type'] == "StopDataRequest"){
                    processor.bufferStatus = "Full";
                    dynCall("vi", CriNcAsr.setRenderingRatioFunc, [105]);
                } else if(event.data['type'] == "CriticalDataRequest"){
                    processor.bufferStatus = "Empty";
                    dynCall("vi", CriNcAsr.setRenderingRatioFunc, [300]);
                }
            };
        } else {
            processor.ncvoice = ncv;
        }
        
        // Use enhanced timer instead of setInterval
        if(CriNcAsr.interval == null && CriNcAsr.mainFunc){
            // Initialize and start the enhanced timer
            CriNcAsr.initEnhancedTimer();
            CriNcAsr.startEnhancedTimer();
            
            // Set a non-null value to prevent multiple starts
            CriNcAsr.interval = true;
        }
    },

    WAASRJS_Start: async function(id) {
        await CriNcAsr.createPromise;
        const processor = CriNcAsr.processors[id];
        if (processor) {
            processor.connect(CriNcAsr.wactx.destination);
        }
    },

    WAASRJS_Stop: async function(id) {
        await CriNcAsr.createPromise;
        const processor = CriNcAsr.processors[id];
        if (processor) {
            processor.disconnect();
        }
    },

    WAASRJS_Destroy: async function(id) {
        await CriNcAsr.createPromise;
        const processor = CriNcAsr.processors[id];
        if (processor) {
            if (CriNcAsr.useWorklet) {
                processor.port.postMessage({ 'type': "Finalize"});
                processor.port.onmessage = null;
            } else {
                processor.onaudioprocess = null;
            }
            processor.disconnect();
            CriNcAsr.processors[id] = null;
            CriNcAsr.ncvoices[id] = null;
        }
    },

    WAASRJS_Finalize: function() {
        // Stop the enhanced timer
        CriNcAsr.stopEnhancedTimer();
        
        CriNcAsr.wactx = null;
        CriNcAsr.initPromise = null;
        CriNcAsr.createPromise = null;
        CriNcAsr.dataCbFunc = null;
        CriNcAsr.mainFunc = null;
        CriNcAsr.interval = null;
    },

    WAASRJS_SetRenderingRatioFunc: function(func) {
        CriNcAsr.setRenderingRatioFunc = func;
    },

    WAASRJS_SetMainFunc: function(cbfunc) {
        CriNcAsr.mainFunc = cbfunc;
    },
    
    WAASRJS_SetServerFrequency: function(serverFrequency) {
        if(serverFrequency > 250) {
            console.warn("Max ADX Server Frequency is 250Hz.");
            serverFrequency = 250;
        }
        CriNcAsr.serverFrequency = serverFrequency;
        
        // If timer is already running, update the target interval
        if (CriNcAsr.timing && CriNcAsr.timing.isRunning) {
            CriNcAsr.timing.targetInterval = 1000 / serverFrequency;
        }
    },

    WAASRJS_GetWorkletStatus: function(id) {
        const processor = CriNcAsr.processors[id];
        if(!processor || CriNcAsr.wactx.state != 'running') {
            return 0;
        }
        
        if(processor.bufferStatus) {
            return processor.bufferStatus == "Empty" ? 1 : 0;
        }
        
        return 0;
    },
    
    WAASRJS_GetWebAudioSamplingRate: function(){
        const isWeChat = (typeof wx !== 'undefined')? true:false;
        var sampleRate;
        if (isWeChat) {
            var AudioContext = wx.createWebAudioContext();
        } else {
            AudioContext = window.AudioContext || window.webkitAudioContext;
        }
        if (AudioContext) {
            var context;
            if(isWeChat){
                context = AudioContext;
            } else {
                context = new AudioContext();
            }
            sampleRate = context.sampleRate;
            context.close();
            return sampleRate;
        }
        console.error("AudioContext is not exist");
        return 44100;
    },
    
    // New API functions for timing control and monitoring
    WAASRJS_GetTimingStats: function() {
        const stats = CriNcAsr.getTimingStats();
        const json = JSON.stringify(stats);
        
        const bufferSize = lengthBytesUTF8(json) + 1;
        const buffer = _malloc(bufferSize);
        stringToUTF8(json, buffer, bufferSize);
        
        return buffer;
    },
    
    WAASRJS_SetTimingMode: function(modePtr) {
        const mode = UTF8ToString(modePtr);
        
        // Adjust timing parameters based on mode
        switch(mode) {
            case 'performance':
                CriNcAsr.timing.urgencyThreshold = 0.6;
                CriNcAsr.timing.adaptiveStepSize = 0.2;
                CriNcAsr.timing.maxConsecutiveMicrotasks = 2;
                console.log("CriNcAsr: Timing mode set to 'performance'");
                break;
                
            case 'balanced':
                CriNcAsr.timing.urgencyThreshold = 0.75;
                CriNcAsr.timing.adaptiveStepSize = 0.1;
                CriNcAsr.timing.maxConsecutiveMicrotasks = 3;
                console.log("CriNcAsr: Timing mode set to 'balanced' (default)");
                break;
                
            case 'quality':
                CriNcAsr.timing.urgencyThreshold = 0.85;
                CriNcAsr.timing.adaptiveStepSize = 0.05;
                CriNcAsr.timing.maxConsecutiveMicrotasks = 5;
                console.log("CriNcAsr: Timing mode set to 'quality'");
                break;
                
            default:
                console.warn(`CriNcAsr: Unknown timing mode '${mode}'`);
        }
    },
};

autoAddDeps(LibraryCriNcAsr, '$CriNcAsr');
mergeInto(LibraryManager.library, LibraryCriNcAsr);