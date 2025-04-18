LibraryCriNc = {
    $CriNc: {
        wactx: null,
        ncvoices: [],
        processors: [],
        interval: null,
        initPromise: null,
		createPromise: null,
        dataCbFunc: null,
        mainFunc: null,
        useWorklet: false,
        serverFrequency: 60,

        createAudioWorklet: async function(id, num_channels) {
            try {
                await CriNc.initPromise;
                
                var audioWorklet = new AudioWorkletNode(CriNc.wactx, 
                    "cri-ncvoice-audio-worklet-processor", 
                    {outputChannelCount:[num_channels]}
                );
                CriNc.processors[id] = audioWorklet;
            } catch (error) {
                console.error("Failed to create AudioWorklet:", error);
            }
        },

        createScriptProcessor: function(id, num_channels) {
            var processor = CriNc.wactx.createScriptProcessor(1024, num_channels, num_channels);
            processor.ringBuffer = [];
            processor.totalStoredSamples = 0;
            processor.offset = 0;
            processor.ncvoice = null;
            
            processor.onaudioprocess = function(e) {
                const output = e.outputBuffer;
                const input = e.inputBuffer;
                var outputOffset = 0;
                
                if(this.totalStoredSamples < 1024 && CriNc.dataCbFunc && this.ncvoice){
                    dynCall("vi", CriNc.dataCbFunc, [this.ncvoice]);
                }
                
                while(true){
                    var remainedOutput = output.length - outputOffset;
                    var buffers = this.ringBuffer.shift();
                    if(buffers == null){
                        for (let channel = 0; channel < output.numberOfChannels; channel++) {
                            const outputData = output.getChannelData(channel);
                            for(let samples = 0; samples < remainedOutput; samples++){
                                outputData[samples + outputOffset] = 0;
                            }
                        }
                        break;
                    }
                    
                    var remainedSamples = buffers[0].length - this.offset;
                    var samplesToCopy = Math.min(remainedOutput, remainedSamples);
                    
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
                    if(outputOffset == output.length){
                        break;
                    }
                }
            };
            
            CriNc.processors[id] = processor;
        }
    },

    WAJS_Initialize: function() {
        var itf = CriNc.itf = Module["CriNcItf"] = Module["CriNcItf"] || {};

        const audioWorkletCode = `class CriNcVoiceAudioWorkletProcessor extends AudioWorkletProcessor {
            constructor() {
                super();
                this.port.onmessage = this.handleMessage.bind(this);
                this.ncvoice = null;
                this.totalStoredSamples = 0;
                this.ringBuffer = [];
                this.offset = 0;
				this.destroyFlag = false;
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
                var outputOffset = 0;
            
                if(this.totalStoredSamples < 1024){
                    this.port.postMessage({ 'type':"DataRequest", 'ncvoice': this.ncvoice});
                }
            
                while(true){
                    var remainedOutput = output[0].length - outputOffset;
                    var buffers = this.ringBuffer.shift();
                    if(buffers == null){
                        for (let channel = 0; channel < output.length; channel++) {
                            for(let samples = 0; samples < remainedOutput; samples++){
                                output[channel][samples + outputOffset] = 0;
                            }
                        }
                        break;
                    }
                    var remainedSamples = buffers[0].length - this.offset;
                    var samplesToCopy = Math.min(remainedOutput, remainedSamples);
                    
                    for (let channel = 0; channel < output.length; channel++) {
                        for(let samples = 0; samples < samplesToCopy; samples++){
                            output[channel][outputOffset + samples] = buffers[channel][this.offset + samples];
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
                    if(outputOffset == output[0].length){
                        break;
                    }
                }
				if(this.destroyFlag == true) return false;
                return true;
            }
        }
        registerProcessor('cri-ncvoice-audio-worklet-processor', CriNcVoiceAudioWorkletProcessor);`;

        if (AudioContext) {
            var context = CriNc.wactx || itf["audioContext"] || new AudioContext({sampleRate: 48000});
            var ua = navigator.userAgent.toLowerCase();
			console.log(ua);
            if (context.audioWorklet && !ua.match(/minigame/)) {
                CriNc.useWorklet = true;
                CriNc.initPromise = context.audioWorklet.addModule('data:text/javascript,' + encodeURI(audioWorkletCode));
            } else {
                CriNc.useWorklet = false;
                CriNc.initPromise = Promise.resolve();
            }
            
            if(navigator.audioSession){ 
                navigator.audioSession.type = 'auto'; 
            }
            
            CriNc.wactx = itf["audioContext"] = context;
            var resume = function(){
                if(CriNc.wactx && CriNc.wactx.state != "running"){
                    CriNc.wactx.suspend();
                    CriNc.wactx.resume();
                }
            };
            window.addEventListener("mousedown", resume);
            window.addEventListener("touchstart", resume);
            document.onvisibilitychange = () => {
                if(!CriNc.wactx) return;
                if (document.visibilityState == "hidden") {
                    CriNc.wactx.suspend();
                } else {
                    setTimeout(()=>{
                        CriNc.wactx.suspend();
                        CriNc.wactx.resume();
                    }, 200);
                }
            }
        }
    },

    WAJS_Create: function(num_channels) {
        var audioProcessor = {};
        CriNc.processors.push(audioProcessor);
        var id = CriNc.processors.indexOf(audioProcessor);
        
        if (CriNc.useWorklet) {
            CriNc.createPromise = CriNc.createAudioWorklet(id, num_channels);
        } else {
            CriNc.createScriptProcessor(id, num_channels);
			CriNc.createPromise = Promise.resolve();
        }

		CriNc.wactx.destination.channelCount = Math.min(num_channels, CriNc.wactx.destination.maxChannelCount);
        
        return id;
    },

    WAJS_PutData: function(id, dataptr, num_samples) {
        const processor = CriNc.processors[id];
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
        
        if (CriNc.useWorklet) {
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

    WAJS_Setup: async function(ncv, id, nch) {
		await CriNc.createPromise;

        const processor = CriNc.processors[id];
        if (!processor) return;
        
        CriNc.ncvoices[id] = ncv;
        
        if (CriNc.useWorklet) {
            processor.channelCount = nch;
            processor.port.postMessage({ 'type': "Init", 'ncvoice': ncv });
            processor.port.onmessage = (event) => {
                if(event.data['type'] == "DataRequest"){
                    if(CriNc.dataCbFunc){
                        dynCall("vi", CriNc.dataCbFunc, [event.data['ncvoice']]);
                    }
                }
            };
        } else {
            processor.ncvoice = ncv;
        }
        
        if(CriNc.interval == null && CriNc.mainFunc){
            CriNc.interval = setInterval(function(){
                dynCall("v", CriNc.mainFunc, []);
            }, Math.ceil(1000 / CriNc.serverFrequency) - 1);
        }
    },

    WAJS_Start: async function(id) {
        await CriNc.createPromise;
        const processor = CriNc.processors[id];
        if (processor) {
            processor.connect(CriNc.wactx.destination);
        }
    },

    WAJS_Stop: async function(id) {
        await CriNc.createPromise;
        const processor = CriNc.processors[id];
        if (processor) {
            processor.disconnect();
        }
    },

    WAJS_Destroy: async function(id) {
        await CriNc.createPromise;
        const processor = CriNc.processors[id];
        if (processor) {
            if (CriNc.useWorklet) {
				processor.port.postMessage({ 'type': "Finalize"});
                processor.port.onmessage = null;
            } else {
                processor.onaudioprocess = null;
            }
            processor.disconnect();
            CriNc.processors[id] = null;
            CriNc.ncvoices[id] = null;
        }
    },

    WAJS_Finalize: function() {
        CriNc.wactx = null;
        CriNc.initPromise = null;
		CriNc.createPromise = null;
        CriNc.dataCbFunc = null;
        CriNc.mainFunc = null;
        clearInterval(CriNc.interval);
        CriNc.interval = null;
    },

    WAJS_SetDataCbFunc: function(cbfunc) {
        CriNc.dataCbFunc = cbfunc;
    },

    WAJS_SetMainFunc: function(cbfunc) {
        CriNc.mainFunc = cbfunc;
    },
    WAJS_SetServerFrequency: function(serverFrequency) {
        if(serverFrequency > 250) {
            console.warn("Max ADX Server Frequency is 250Hz.");
            serverFrequency = 250;
        }
        CriNc.serverFrequency = serverFrequency;
    },
};

autoAddDeps(LibraryCriNc, '$CriNc');
mergeInto(LibraryManager.library, LibraryCriNc);