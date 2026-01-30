LibraryCriNc = {

$CriNc: {
	wactx: null,	// WebAudioのAudioContext
	voices: [null],	// ボイス(0==nullを避けるため、先頭にnullを置いておく)
	buses: [],		// バス(0～7)
	buffers: {},	// ロードしたAudioBufferが全部入っている
	bufferCount: 0,	// 波形をロードした回数(ユニークIDに使用)
	preloaded: {},	// 事前ロードしたAudioBuffer
	banks: {},		// ロードしたACBのAudioBufferが入っている
	bankCount: 0,	// ACBをロードした回数(ユニークIDに使用)
	browser: null,	// クライアントのブラウザの種類
	os: null,		// クライアントのOSの種類
	suspended: false,	// suspend()によりオーディオがサスペンドされている
	unlocked: false,	// ページロード直後でオーディオがアンロック状態
	extnodes: [null],	// 拡張フィルタノード(0==nullを避けるため、先頭にnullを置いておく)
	analyzers: [],		// 信号解析器(レベルメータ、スペクトルアナライザ)
	worker: null,
	workerError: null,

	setupWebAudio: function() {
		var isEdge = CriNc.browser == 'edge';

		CriNc.applySuspendState = function() {
			var context = CriNc.wactx;
			if (CriNc.suspended && context.suspend && context.state === "running") {
				context.suspend()
				.then(CriNc.applySuspendState)
				.catch(CriNc.applySuspendStateRejected);
			}
			if (!CriNc.suspended && context.resume && context.state === "suspended") {
				context.resume()
				.then(CriNc.applySuspendState)
				.catch(CriNc.applySuspendStateRejected);
			}
		};
		CriNc.applySuspendStateRejected = function() {
			setTimeout(CriNc.applySuspendState, 100);
		};

		// バスのレベル解析器の処理
		CriNc.updateAnalyzers = function() {
			var currentTime = CriNc.wactx.currentTime;
			for (var i = 0; i < CriNc.analyzers.length; i++) {
				var analyzer = CriNc.analyzers[i];
				analyzer.update(currentTime);
			}
		};
		CriNc.Analyzer = function(interval, peakholdTime) {
			var node = CriNc.wactx.createAnalyser();
			node.smoothingTimeConstant = 0;
			node.fftSize = 1024;
			this.node = node;
			this.analyzingLastTime = 0;
			this.analyzingInterval = interval / 1000.0;
			this.peakholdLastTime = 0;
			this.peakholdTime = peakholdTime / 1000.0;
			this.rmsLevel = 0;
			this.peakLevel = 0;
			this.peakholdLevel = 0;
			this.sumSqu = 0;
			this.peakSqu = 0;
		};
		CriNc.Analyzer.prototype = {
			destroy: function() {
				this.node.disconnect();
			},
			update: function(currentTime) {
				var fftsize = this.node.fftSize;
				var sumSqu = 0.0, peakSqu = 0.0;

				var stack = stackSave();
				var pcm = new Float32Array(wasmMemory.buffer, stackAlloc(fftsize * 4), fftsize);
				this.getTimeDomainData(pcm);
				for (var j = 0; j < fftsize; j++) {
					var s = pcm[j];
					var squ = s * s;
					sumSqu += squ;
					if (squ > peakSqu) peakSqu = squ;
				}
				stackRestore(stack);

				// ピーク、RMS値の更新
				this.peakSqu = Math.max(this.peakSqu, peakSqu);
				this.sumSqu += sumSqu;
				var elapsedTime = currentTime - this.analyzingLastTime;
				if (elapsedTime >= this.analyzingInterval) {
					this.peakLevel = Math.sqrt(this.peakSqu);
					this.rmsLevel = Math.sqrt(this.sumSqu / (elapsedTime * CriNc.wactx.sampleRate));
					this.analyzingLastTime = currentTime;
					// ピークホールド値の更新
					if (this.peakLevel > this.peakholdLevel) {
						this.peakholdLevel = this.peakLevel;
						this.peakholdLastTime = currentTime;
					}
					this.peakSqu = 0;
					this.sumSqu = 0;
				}
				// ピークホールドの下がり処理
				elapsedTime = currentTime - this.peakholdLastTime;
				if (elapsedTime >= this.peakholdTime) {
					this.peakholdLevel += (this.peakLevel - this.peakholdLevel) / 12.0;
					if (this.peakholdLevel < 0.000001) {
						this.peakholdLevel = 0;
					}
				}
			},
			getFrequencyData: function(buffer) {
				this.node.getFloatFrequencyData(buffer);
			},
			getTimeDomainData: function(buffer) {
				var node = this.node;
				if (node.getFloatTimeDomainData) {
					node.getFloatTimeDomainData(buffer);
				} else {
					var length = node.fftSize;
					var stack = stackSave();
					var u8buf = new Uint8Array(buffer, stackAlloc(length), length);
					node.getByteTimeDomainData(u8buf);
					for (var j = 0; j < length; j++) {
						buffer[j] = (u8buf[j] - 128) / 128.0;
					}
					stackRestore(stack);
				}
			},
		};
		function setParam(param, value) {
			if (true) {
				// Edgeは不具合回避のためスケジュール設定を行わない
				param.value = value;
			} else {
				param.cancelScheduledValues(0);
				param.setValueAtTime(value, 0);
				param.lastValue = value;
			}
		}
		function transitionParam(param, value, time, duration) {
			if (true) {
				// Edgeは不具合回避のためスケジュール設定を行わない
				param.value = value;
			} else {
				// iOS 14.2 以降、macOS Safari 14.0 以降において、
				// 引数バリデートが厳格化され、param.lastValueがundefinedだと
				// エラーとなる不具合を修正。
				param.linearRampToValueAtTime(param.lastValue == null ? 0 : param.lastValue, time);
				param.linearRampToValueAtTime(value, time + duration);
				param.lastValue = value;
			}
		}
		var bqfTypeTable = [
			"lowpass",
			"highpass",
			"notch",
			"lowshelf",
			"highshelf",
			"peaking",
		];
		function updateBpf(bpf, cofLow, cofHigh) {
			// バンドパスパラメータからバイクワッドパラメータに変換
			var maxFreq = CriNc.wactx.sampleRate / 2;
			var cof1 = Math.min(Math.max(cofLow, 20), maxFreq);
			var cof2 = Math.min(Math.max(cofHigh, 20), maxFreq);
			if (cof1 > cof2 - 10) cof1 = cof2 - 10;
			var bw = Math.log(cof2 / cof1) / Math.log(2);
			var f0 = cof1 * Math.pow(2, bw / 2);
			setParam(bpf.frequency, f0);
			setParam(bpf.Q, f0 / (cof2 - cof1));
		}
		/**
		 * WebAudioボイス
		 * @constructor
		 */
		CriNc.Voice = function(wactx, inCh, outCh) {
			this.decoder = null;			// 外部から渡されるデコーダ
			this.playing = false;			// 再生中フラグ
			this.releasing = false;			// 停止中フラグ
			this.paused = false;			// 一時停止されている
			this.rate = 0;					// 再生速度レート
			this.predelay = 0;				// プリディレイ
			this.startOffset = 0;			// シーク時間
			this.samples = 0;				// 再生済サンプル数
			this.stopTime = 0;				// 停止する時刻
			this.lastTime = 0;				// 最後にupdateされた時刻
			this.envTime = 0;				// Sustainまでのエンベロープ時間
			this.envActive = false;			// エンベロープ有効フラグ
			this.envAtk = 0;				// アタック時間
			this.envHld = 0;				// ホールド時間
			this.envDcy = 0;				// ディケイ時間
			this.envRel = 0;				// リリース時間
			this.envSus = 1;				// サスティンレベル

			this.source = null;				// AudioBufferSourceNode
			this.waveData = null;			// WaveData
			this.filterNode = null;			// フィルタコールバックの代替ノード
			this.loopLimit = 0;
			this.aux = null;
			this.numChannels = inCh;

			// バンドパスフィルタ
			this.bpf = {
				node: null,
				cofLo: 0,				// バンドパス低域カット周波数
				cofHi: 24000,			// バンドパス高域カット周波数
			};
			// バイクワッドフィルタ
			this.biq = {
				node: null,
				type: 0,
				freq: 0,
				Q: 1,
				gain: 1
			};

			// エンベロープのレベル変化、ポーズ時のフェード処理に使うGainNode
			this.level = wactx.createGain();
			// マトリクスレベル処理
			this.matrix = new CriNc.Matrix(wactx, inCh, outCh);
			// バスセンドのルート処理
			this.router = new CriNc.Router(wactx, 8);

			// 初期化
			this.cleanup();
		};
		// ボイスの関数
		CriNc.Voice.prototype = {
			setData: function(waveData) {
				this.waveData = waveData;
			},
			setup: function(startOffset, loopLimit, filterNode) {
				this.startOffset = startOffset;
				this.loopLimit = loopLimit;
				this.filterNode = filterNode;
				if (this.source) {
					this.cleanup();
				}
			},
			cleanup: function() {
				if (this.source) {
					this.source.disconnect();
					this.source.onended = null;
					this.source = null;
				}
				if (this.biq.node) {
					this.biq.node.disconnect();
					this.biq.node = null;
				}
				if (this.bpf.node) {
					this.bpf.node.disconnect();
					this.bpf.node = null;
				}
				if (this.filterNode) {
					if (this.filterNode.node) {
						this.filterNode.node.disconnect();
						this.filterNode.node = null;
					}
					this.filterNode = null;
				}
				if (this.aux) {
					this.aux.cleanup();
				}
				this.level.gain.cancelScheduledValues(0);
				this.level.disconnect();
				this.level.next = null;
				this.matrix.reset();
				this.router.reset(this.matrix.merge);
				this.waveData = null;
				this.playing = false;
				this.releasing = false;
			},
			setSamplingRate: function(rate) {
				this.rate = rate;
				this.updatePlaybackRate();
			},
			updatePlaybackRate: function() {
				if (!this.waveData) return;
				var rate = 0;
				if (this.paused) {
					// 0だとFirefoxで止まらないのでゼロに近い値をセット
					rate = 0.00000001;
				} else {
					rate = +this.rate / this.waveData.originalSampleRate;
				}
				setParam(this.source.playbackRate, rate);
			},
			start: function(type) {
				if (this.playing) {
					return;
				}
				var waveData = this.waveData;
				var startOffset = this.startOffset;
				var source;

				if(type == 1){
					this.aux = new CriNc.Aux();
					this.aux.numChannels = this.numChannels;
					source = this.source = this.aux.worklet = new AudioWorkletNode(CriNc.wactx, 'AuxAudioWorklet', {outputChannelCount: [this.numChannels]});
					this.aux.start();
				} else {
					source = this.source = CriNc.wactx.createBufferSource();

					// バッファをセット
					source.buffer = waveData.buffer;
	
					// ループ設定
					var loopLimit = this.loopLimit;
					switch (loopLimit) {
					case -1: loopLimit = (waveData.loopEmbd) ? Number.POSITIVE_INFINITY : 0; break;
					case -2: loopLimit = 0; break;
					case -3: loopLimit = Number.POSITIVE_INFINITY; break;
					}
					if (loopLimit > 0) {
						// ループ情報をセット
						source.loop = true;
						source.loopStart = waveData.loopStart;
						source.loopEnd = waveData.loopEnd;
						// シーク位置計算
						if (startOffset >= waveData.loopEnd) {
							startOffset = waveData.loopStart +
								(startOffset - waveData.loopStart) %
								(waveData.loopEnd - waveData.loopStart);
						}
					}
					this.loopLimit = loopLimit;
					startOffset += waveData.offset;
	
					// 再生終了コールバック
					source.onended = function() {
						this.cleanup();
					}.bind(this);
				}

				var node = source;
				if (this.filterNode) {
					node.connect(this.filterNode.node);
					node = this.filterNode.node;
				}
				node.connect(this.level);

				this.updateFilters();
				this.updatePlaybackRate();

				var time = CriNc.wactx.currentTime + this.predelay;
				var startTime = time;
				var gain = this.level.gain;

				this.samples = 0;
				this.releasing = false;
				this.playing = true;
				this.envTime = startTime;
				this.lastTime = startTime;

				// エンベロープ制御
				if (this.envActive) {
					// アタック
					if (this.envAtk > 0) {
						gain.setValueAtTime(0, time);
						time += this.envAtk;
						gain.linearRampToValueAtTime(1, time);
					} else {
						gain.setValueAtTime(1, time);
					}
					// ホールド
					if (this.envHld > 0) {
						time += this.envHld;
					}
					// ディケイ
					if (this.envDcy > 0) {
						gain.setValueAtTime(1, time);
						time += this.envDcy;
						gain.linearRampToValueAtTime(this.envSus, time);
					}
				} else {
					gain.setValueAtTime(1, startTime);
				}

				// 開始リクエスト
				if(type != 1){
					if (source.loop) {
						source.start(startTime, startOffset);
					} else {
						source.start(startTime, startOffset, waveData.duration);
					}
				}
			},
			stop: function() {
				if (!this.playing) {
					return;
				}
				if (this.paused || this.releasing) {
					// ポーズ中、リリース中なら即時停止
					this.stopNode(false);
					this.paused = false;
				} else {
					// リリースを行う
					this.stopNode(this.envActive);
				}
			},
			stopNode: function(shouldRelease) {
				// 停止時間を計算
				var time = CriNc.wactx.currentTime;
				this.level.gain.cancelScheduledValues(0);
				this.level.gain.setValueAtTime(this.getEnvLevel(), time);
				this.envTime = time;
				time += (shouldRelease) ? this.envRel : 0.02;
				this.level.gain.linearRampToValueAtTime(0, time);
				// 停止リクエスト
				this.source.stop(time);
				this.stopTime = time;
				if (shouldRelease) {
					this.releasing = true;
				} else {
					this.playing = false;
				}
			},
			update: function() {
				// 時刻の更新
				var currentTime = CriNc.wactx.currentTime;
				if (this.playing && !this.paused) {
					var waveData = this.waveData;
					var elapsedTime = currentTime - this.lastTime;
					if (elapsedTime >= 0) {
						this.samples += elapsedTime * this.rate;
					}

					if (this.source && this.source.loop) {
						// ループ回数制限
						var time = this.samples / waveData.originalSampleRate;
						var loopCount = ((time - waveData.loopStart) / (waveData.loopEnd - waveData.loopStart))|0;
						if (loopCount >= this.loopLimit) this.source.loop = false;
					}
				}
				this.lastTime = currentTime;


				// 停止時間を過ぎていたら停止
				if (this.releasing && currentTime >= this.stopTime) {
					this.cleanup();
				}
			},
			getEnvLevel: function() {
				// エンベロープタイムを計算
				var time = Math.max(0, CriNc.wactx.currentTime - this.envTime);
				if (this.paused) {
					return 0;
				}
				if (this.releasing) {
					// リリース中のレベルを計算
					if (time < this.envRel) return this.envSus * (1.0 - time / this.envRel);
					return 0;
				} else {
					// アタック,ホールド,ディケイ中のレベルを計算
					if (time < this.envAtk) return time / this.envAtk;
					time -= this.envAtk;
					if (time < this.envHld) return 1.0;
					time -= this.envHld;
					if (time < this.envDcy) return 1.0 - (1.0 - this.envSus) * time / this.envDcy;
					return this.envSus;
				}
			},
			pause: function(paused) {
				// 再生中でない時、ポーズ状態が変更されない時は抜ける
				if (!this.playing || paused == this.paused) {
					return;
				}
				// リリース中のポーズは停止
				if (this.releasing) {
					this.stopNode(false);
					return;
				}
				this.paused = paused;

				// 短い時間でレベルをフェードイン/アウトさせてプチノイズを回避
				var levelNode = this.level.gain;
				var beginLevel, endLevel;
				if (paused) {
					// レベルを下げる
					beginLevel = this.envSus;
					endLevel = 0;
				} else {
					// レベルを戻す
					beginLevel = 0;
					endLevel = this.envSus;
				}
				var time = CriNc.wactx.currentTime;
				levelNode.cancelScheduledValues(0);
				levelNode.setValueAtTime(beginLevel, time);
				levelNode.linearRampToValueAtTime(endLevel, time + 0.02);

				// 再生レートをゼロにしてポーズを実現するので更新をかける
				this.updatePlaybackRate();
			},
			getTime: function(count, tunit) {
				// ポインタに現在時刻を返す
				if (this.waveData) {
					setValue(count, this.samples, "i64");
					setValue(tunit, this.waveData.originalSampleRate, "i32");
				}
			},
			setOutputMatrix: function(nch, nspk, matrix) {
				var isPlaying = this.playing;
				var gains = this.matrix.gains;
				nch  = Math.min(nch, gains.length / 2)|0;
				nspk = Math.min(nspk, 2)|0;
				var time = CriNc.wactx.currentTime + 0.01;
				for (var i = 0; i < nch; i++) {
					// メモリからポインタを取得
					var ptr = HEAPU32[(matrix>>2) + i];
					for (var j = 0; j < nspk; j++) {
						// メモリからレベルを取得
						var level = HEAPF32[(ptr>>2) + j];
						// 設定対象のゲインノードを取得
						var node = gains[i * 2 + j].gain;
						if (isPlaying) {
							transitionParam(node, level, time, 0.01);
						} else {
							setParam(node, level);
						}
					}
				}
			},
			// プリディレイの設定
			setPreDelay: function(time) {
				this.predelay = time;
			},
			// エンベロープのスイッチ
			setEnvActive: function(active) {
				this.envActive = active;
				// リリース中のエンベロープOFFは停止
				if (!active && this.releasing) {
					this.stopNode(false);
				}
			},
			// エンベロープパラメータの設定
			setEnvParam: function(paramId, value) {
				switch (paramId) {
				case 0: this.envAtk = value * 0.001; break;
				case 1: this.envHld = value * 0.001; break;
				case 2: this.envDcy = value * 0.001; break;
				case 3: this.envRel = value * 0.001; break;
				case 4: this.envSus = value; break;
				}
			},
			// バイクアッドフィルタのスイッチ
			setBiqActive: function(active) {
				if (!active) {
					this.biq.node = null;
				} else if (!this.biq.node) {
					this.biq.node = CriNc.wactx.createBiquadFilter();
				}
				this.updateFilters();
			},
			// バイクアッドフィルタのタイプ設定
			setBiqType: function(value) {
				this.biq.type = value;
			},
			// バイクアッドフィルタの周波数設定
			setBiqFreq: function(value) {
				this.biq.freq = value;
			},
			// バイクアッドフィルタのQ値設定
			setBiqQ: function(value) {
				this.biq.Q = value;
			},
			// バイクアッドフィルタのゲイン設定
			setBiqGain: function(value) {
				this.biq.gain = value;
			},
			// バイクアッドフィルタの更新
			updateBiq: function() {
				var biq = this.biq;
				var node = biq.node;
				var maxFreq = CriNc.wactx.sampleRate / 2;
				var cof = Math.min(Math.max(biq.freq, 20), maxFreq);
				setParam(node.frequency, cof);
				setParam(node.Q, biq.Q);
				setParam(node.gain, biq.gain);
				node.type = bqfTypeTable[biq.type];
			},
			// バンドパスフィルタのスイッチ
			setBpfActive: function(active) {
				if (!active) {
					this.bpf.node = null;
				} else if (!this.bpf.node) {
					this.bpf.node = CriNc.wactx.createBiquadFilter();
					this.bpf.node.type = "bandpass";
				}
				this.updateFilters();
			},
			// バンドパスフィルタの低域カットオフ周波数の設定
			setBpfCofLo: function(value) {
				this.bpf.cofLo = value;
			},
			// バンドパスフィルタの高域カットオフ周波数の設定
			setBpfCofHi: function(value) {
				this.bpf.cofHi = value;
			},
			// バンドパスフィルタの更新
			updateBpf: function() {
				updateBpf(this.bpf.node, this.bpf.cofLo, this.bpf.cofHi);
			},
			// DSP関連(バイクワッド、バンドパス、エンベロープ、プリディレイ)のリセット
			resetDspParams: function() {
				// バイクワッドフィルタを初期化
				this.biq.type = 0;
				this.biq.freq = 0;
				this.biq.Q = 1;
				this.biq.gain = 1;

				// バンドパスフィルタを初期化
				this.bpf.cofLo = 0;
				this.bpf.cofHi = 24000;

				// エンベロープを初期化
				this.envActive = false;
				this.envAtk = 0;
				this.envHld = 0;
				this.envDcy = 0;
				this.envRel = 0;
				this.envSus = 1.0;

				// プリディレイを初期化
				this.predelay = 0;
			},
			// フィルタ関連の更新
			updateFilters: function() {
				var node = this.level;
				
				var biq = this.biq.node;
				if (biq) {
					// バイクワッドフィルタの接続
					if (node.next !== biq) {
						node.disconnect();
						node.connect(biq);
						node.next = biq;
					}
					node = biq;
				}

				var bpf = this.bpf.node;
				if (bpf) {
					// バンドパスフィルタの接続
					if (node.next !== bpf) {
						node.disconnect();
						node.connect(bpf);
						node.next = bpf;
					}
					node = bpf;
				}
				// パンへの接続
				var output = this.matrix.split;
				if (node.next !== output) {
					node.disconnect();
					node.connect(output);
					node.next = output;
				}
			},
			setRouting: function(busId, level) {
				this.router.set(busId, level, this.matrix.merge);
			},
			resetRouting: function() {
				var node = this.matrix.merge;
				this.router.reset(node);
				node.disconnect();
			},
		};
		/**
		 * バス
		 * @constructor
		 */
		CriNc.Bus = function(wactx, isMasterBus) {
			this.fx = [];
			this.input = wactx.createGain();
			this.level = wactx.createGain();
			this.output = null;
			this.filters = [];
			if (!isMasterBus) {
				this.router = new CriNc.Router(wactx, 8);
			}
			this.update();
		};
		// バスの関数
		CriNc.Bus.prototype = {
			// FXを空きスロットに追加
			attachFx: function(fx) {
				this.fx.push(fx);
			},
			// 全てのFXを破棄
			detachAllFx: function() {
				this.input.disconnect();
				this.input.next = null;
				for (var i in this.fx) {
					var fx = this.fx[i];
					fx.output.disconnect();
					fx.output.next = null;
				}
				this.fx.length = 0;
				this.update();
			},
			// FXをIdで探す
			findFx: function(fxId) {
				for (var i in this.fx) {
					var fx = this.fx[i];
					if (fx.id == fxId) {
						return fx;
					}
				}
			},
			// バス内ノードの接続を更新する
			update: function() {
				var node = this.input;
				for (var i in this.fx) {
					var fx = this.fx[i];
					if (fx.bypass) {
						// FXのバイパス
						if (node.next) {
							// バイパス設定されたので切断
							node.disconnect(node.next);
							node.next = null;
						}
					} else if (node.next !== fx.input) {
						// 接続が違うので再設定
						if (node.next) {
							// 既に存在する接続を切断
							node.disconnect(node.next);
						}
						// 新しい接続
						node.connect(fx.input);
						node.next = fx.input;
						node = fx.output;
					}
				}
				// ボリューム設定用ノードに接続
				node.connect(this.level);
				node.next = this.level;
				node = this.level;

				// フィルタノードの接続
				for (var i in this.filters) {
					var filter = this.filters[i];
					if (node.next !== filter.node) {
						node.disconnect();
						node.connect(filter.node);
						node.next = filter.node;
					}
					node = filter.node;
				}

				// 出力ノードの接続
				if (this.output !== node) {
					node.disconnect();
					this.output = node;
					if (this.router) {
						// バスルーティング
						for (var i in this.router.levels) {
							var target = this.router.levels[i];
							if (target) node.connect(target);
						}
					} else {
						// オーディオ出力へ送る
						node.connect(CriNc.wactx.destination);
					}
				}
			},
			setVolume: function(volume) {
				setParam(this.level.gain, volume);
			},
			setRouting: function(busId, level) {
				if (this.router) {
					this.router.set(busId, level, this.output);
				}
			},
			resetRouting: function() {
				if (this.router) {
					this.router.reset(this.output);
				}
			},
			attachFilter: function(filter) {
				this.filters.push(filter);
				this.update();
			},
			detachFilter: function(filter) {
				var idx = this.filters.indexOf(filter);
				if (idx >= 0) {
					this.filters.splice(idx, 1);
					this.update();
				}
			}
		},
		/**
		 * パンニング用マトリクス
		 * @constructor
		 */
		CriNc.Matrix = function(wactx, inCh, outCh) {
			this.split = wactx.createChannelSplitter(inCh);
			this.merge = wactx.createChannelMerger(outCh);
			this.gains = Array(inCh * outCh);
			for (var i = 0; i < inCh; i++) {
				for (var j = 0; j < outCh; j++) {
					var gain = wactx.createGain();
					this.split.connect(gain, i, 0);
					gain.connect(this.merge, 0, j);
					this.gains[i * outCh + j] = gain;
				}
			}
		};
		CriNc.Matrix.prototype = {
			reset: function() {
				for (var i = 0; i < this.gains.length; i++) {
					var node = this.gains[i].gain;
					setParam(node, 0);
				}
			}
		};
		/**
		 * バスルーティング用
		 * @constructor
		 */
		CriNc.Router = function(wactx, count) {
			this.levels = Array(count);
			this.targets = Array(count);
			this.reset();
		};
		CriNc.Router.prototype = {
			reset: function(source) {
				for (var i in this.levels) {
					var node = this.levels[i];
					if (node) {
						node.disconnect();
						source.disconnect(node);
					}
				}
				this.targets.fill(-1);
				this.levels.fill(null);
			},
			set: function(target, level, source) {
				// センド先が登録されているか探す
				var index = this.targets.indexOf(target);
				if (index == -1) {
					// 見つからなければ接続する
					if (level <= 0) {
						return;
					}
					index = this.targets.indexOf(-1);
					if (index < 0) {
						return;
					}
					// GainNodeが無ければ作成する
					if (!this.levels[index]) {
						this.levels[index] = CriNc.wactx.createGain();
					}
					this.targets[index] = target;
					if (target < CriNc.buses.length) {
						// バスへ接続
						this.levels[index].connect(CriNc.buses[target].input);
					}
					// バスセンドのノードにソースを接続
					source.connect(this.levels[index]);
				}

				// レベル調整
				var node = this.levels[index];
				node.gain.cancelScheduledValues(0);
				node.gain.setValueAtTime(level, CriNc.wactx.currentTime);
			},
		};
		var PROCESS_UNIT = 1024;
		/**
		 * @constructor
		 */
		function Dly(delay) {
			delay = delay|0;
			var dbuf = new Float32Array(delay);
			var rpos = 0, wpos = 0;
			this.process = function(ibuf, obuf) {
				var length = ibuf.length;
				for (var i = 0;  i < length; i++) {
					var ival = ibuf[i];
					var dval = dbuf[rpos];
					dbuf[wpos] = ival;
					obuf[i] = dval;
					if (++rpos >= delay) rpos = 0;
					if (++wpos >= delay) wpos = 0;
				}
			};
			this.read = function(obuf) {
				var length = obuf.length;
				for (var i = 0;  i < length; i++) {
					obuf[i] = dbuf[rpos];
					if (++rpos >= delay) rpos = 0;
				}
			}
			this.write = function(ibuf) {
				var length = ibuf.length;
				for (var i = 0;  i < length; i++) {
					dbuf[wpos] = ibuf[i];
					if (++wpos >= delay) wpos = 0;
				}
			}
		}
		/**
		 * @constructor
		 */
		function Apf(delay, g1) {
			delay = delay|0;
			var dbuf = new Float32Array(delay);
			var pos = 0;
			var g2 = 1.0 - g1 * g1;
			this.process = function(ibuf, obuf) {
				var length = ibuf.length;
				for (var i = 0;  i < length; i++) {
					var ival = ibuf[i];
					var dval = dbuf[pos];
					dbuf[pos] = ival + dval * g1;
					obuf[i] = ival * -g1 + dval * g2;
					if (++pos >= delay) pos = 0;
				}
			};
		}
		/**
		 * @constructor
		 */
		function Rsf(delay, g, sfreq, lf, hf) {
			delay = delay|0;
			var dbuf = new Float32Array(delay);
			var pos = 0;
			var flt = calcBpf(sfreq, lf, hf);
			var x0=0, x1=0, x2=0;
			var y0=0, y1=0, y2=0;
			this.process = function(ibuf, obuf) {
				var length = ibuf.length;
				var ra=flt.a[0];
				var b0=flt.b[0]/ra, b1=flt.b[1]/ra, b2=flt.b[2]/ra;
				var a1=flt.a[1]/ra, a2=flt.a[2]/ra;
				for (var i = 0;  i < length; i++) {
					x0 = dbuf[pos];
					y0 = b0 * x0 + b1 * x1 + b2 * x2 - a1 * y1 - a2 * y2;

					x2 = x1; x1 = x0;
					y2 = y1; y1 = y0;

					dbuf[pos] = ibuf[i] + y0 * g;
					obuf[i] = x0;

					if (++pos >= delay) pos = 0;
				}
			};
		}
		/**
		 * @constructor
		 */
		function Iir(flt) {
			var x0=0, x1=0, x2=0;
			var y0=0, y1=0, y2=0;
			this.process = function(ibuf, obuf) {
				var length = ibuf.length;
				var ra = flt.a[0];
				var b0=flt.b[0]/ra, b1=flt.b[1]/ra, b2=flt.b[2]/ra;
				var a1=flt.a[1]/ra, a2=flt.a[2]/ra;
				for (var i = 0;  i < length; i++) {
					x0 = ibuf[i];
					y0 = b0 * x0 + b1 * x1 + b2 * x2 - a1 * y1 - a2 * y2;
					x2 = x1; x1 = x0;
					y2 = y1; y1 = y0;
					obuf[i] = y0;
				}
			};
		}
		function calcBpf(sfreq, cof1, cof2) {
			function cmplx(re, im) {
				return {re: re, im: im};
			}
			function add(c1, c2){
				return {re: c1.re+c2.re,
						im: c1.im+c2.im};
			}
			function div(c1, c2){
				var scale = 1.0/(c2.re*c2.re + c2.im*c2.im);
				return {re: ( c1.re*c2.re + c1.im*c2.im) * scale,
						im: (-c1.re*c2.im + c1.im*c2.re) * scale};
			}
			function mul(c1, c2){
				return {re: c1.re*c2.re - c1.im*c2.im,
						im: c1.re*c2.im + c1.im*c2.re};
			}
			function mulgain(c1, gain){
				return {re: c1.re*gain,
						im: c1.im*gain};
			}
			var PI = Math.PI;
			var ts = 1.0 / sfreq;
			var cof1a = 1.0 / (PI * ts) * Math.tan(PI * ts * cof1);
			var cof2a = 1.0 / (PI * ts) * Math.tan(PI * ts * cof2);
			var w1 = 2.0 * PI * cof1a;
			var w2 = 2.0 * PI * cof2a;
			var w0 = Math.sqrt(w1 * w2);
			var bw = w2 - w1;
			var tmp = bw * bw - 4.0 * w0 * w0;
			var pla = Array(2);
			var zra = Array(2);
			if (tmp > 0) {
				pla[0] = cmplx((-bw+Math.sqrt(tmp))/2.0, 0.0);
				pla[1] = cmplx((-bw-Math.sqrt(tmp))/2.0, 0.0);
			} else {
				pla[0] = cmplx(-bw/2.0,  Math.sqrt(-tmp)/2.0);
				pla[1] = cmplx(-bw/2.0, -Math.sqrt(-tmp)/2.0);
			}
			zra[0] = {re:0.0, im:0.0};

			var pld = Array(3);
			var zrd = Array(3);

			var g = cmplx(bw,0.0);
			var t = cmplx(ts,0.0);
			for (var i=0; i<2; i++) {
				var b = cmplx(2.0 - pla[i].re*ts, -pla[i].im*ts);
				var k = div(t, b);
				g = mul(g, k);
				var c = cmplx(2.0 + pla[i].re*ts, pla[i].im*ts);
				pld[i] = div(c, b);
			}
			for (i=0; i<1; i++) {
				var b = cmplx(2.0 - zra[i].re*ts, -zra[i].im*ts);
				var k = div(b, t);
				g = mul(g, k);
				var c = cmplx(2.0 + zra[i].re*ts, zra[i].im*ts);
				zrd[i] = div(c, b);
			}
			var gaind = g.re;
			zrd[1] = cmplx(-1.0, 0.0);

			function calcCoef(x, g) {
				var a = [cmplx(1.0,0.0),
				         cmplx(0.0,0.0),
				         cmplx(0.0,0.0)];
				for (var n=1; n<=2; n++) {
					for (var i=n; i>0; i--) {
						var t1 = mulgain(x[n-1], -1.0);
						var t2 = mul(a[i-1], t1);
						a[i] = add(a[i], t2);
					}
				}
				return [a[0].re*g, a[1].re*g, a[2].re*g];
			}
			var a = calcCoef(pld, 1.0);
			var b = calcCoef(zrd, gaind);
			return {a: a, b: b};
		}
		function calcLsf(sfreq, cof, q, gain) {
			q = Math.max(q, 0.001);
			cof = Math.min(Math.max(cof, 10.0), sfreq / 2.0 - 100);
			var w0 = 2.0 * Math.PI * cof / sfreq;
			var sw = Math.sin(w0);
			var cw = Math.cos(w0);
			var ea = Math.sqrt(Math.max(gain, 1/65536.0));
			var alpha = sw / (2.0 * q);
			var sqrt_a = 2.0 * Math.sqrt(ea) * alpha;

			return {
			b:[         ea * ((ea + 1.0) - (ea - 1.0) * cw + sqrt_a)
			  ,   2.0 * ea * ((ea - 1.0) - (ea + 1.0) * cw)
			  ,         ea * ((ea + 1.0) - (ea - 1.0) * cw - sqrt_a)
			],
			a:[               (ea + 1.0) + (ea - 1.0) * cw + sqrt_a
			  ,  -2.0 *      ((ea - 1.0) + (ea + 1.0) * cw)
			  ,               (ea + 1.0) + (ea - 1.0) * cw - sqrt_a
			]};
		}
		function calcHsf(sfreq, cof, q, gain) {
			q = Math.max(q, 0.001);
			cof = Math.min(Math.max(cof, 10.0), sfreq / 2.0 - 100);
			var w0 = 2.0 * Math.PI * cof / sfreq;
			var sw = Math.sin(w0);
			var cw = Math.cos(w0);
			var ea = Math.sqrt(Math.max(gain, 1/65536.0));
			var alpha = sw / (2.0 * q);
			var sqrt_a = 2.0 * Math.sqrt(ea) * alpha;

			return {
			b:[        ea * ((ea + 1.0) + (ea - 1.0) * cw + sqrt_a)
			  , -2.0 * ea * ((ea - 1.0) + (ea + 1.0) * cw)
			  ,        ea * ((ea + 1.0) + (ea - 1.0) * cw - sqrt_a)
			],
			a:[              (ea + 1.0) - (ea - 1.0) * cw + sqrt_a
			  ,  2.0      * ((ea - 1.0) - (ea + 1.0) * cw)
			  ,              (ea + 1.0) - (ea - 1.0) * cw - sqrt_a
			]};
		}

		/**
		 * @constructor
		 */
		function Reverb(sfreq, params) {
			var reverbtime = params[0];
			var roomsize = params[1];
			var predelay = params[2];
			var lf = params[3];
			var hf = params[4];

			var pdly = new Dly(predelay * sfreq / 1000);
			var apfdly = [7.80, 1.31, 6.53, 9.52, 3.75];
			var apf = Array(5);
			for (var i = 0; i < 5; i++) {
				apf[i] = new Apf(apfdly[i]*sfreq/1000, 0.61);
			}

			var rsfdly = [1.0 + 0.02410692,1.0 - 0.355858106,1.0 - 0.299650262,1.0 - 0.216462653,1.0 - 0.086060455,1.0 + 0.165750687,1.0 + 0.397327005,1.0 + 0.278166375,1.0 + 0.09268049];
			var rsfcof = [1.0 - 0.247148289,1.0 + 0.02661597,1.0 - 0.144486692,1.0 + 0.368821293,1.0 - 0.007604563,1.0 + 0.129277567,1.0 - 0.076045627,1.0 - 0.212927757,1.0 + 0.163498099];

			function primaryTime(time) {
				var nsmpl = (time * sfreq / 1000.0)|0;
				var check_max = nsmpl / 2 + 1;
				for (var i = 3; i < check_max; i += 2) {
					if ((nsmpl % i) == 0) {
						nsmpl++;
						i = 3;
						check_max = nsmpl / 2 + 1;
					}
				}
				return nsmpl;
			}

			var fbtime = roomsize / 334.0 * 1000.0;
			var rsf = Array(9);
			for (i = 0; i < 9; i++) {
				var delaytime = primaryTime(fbtime * rsfdly[i]);
				var gain = Math.pow(0.001, (delaytime * 1000 / sfreq) / reverbtime);
				rsf[i] = new Rsf(delaytime, gain, sfreq, lf * rsfcof[i], hf * rsfcof[i]);
			}

			var rbuf = new Float32Array(PROCESS_UNIT);
			this.process = function(ibuf, obufL, obufR) {
				var length = ibuf.length;

				pdly.process(ibuf, ibuf);

				for (var i = 0; i < 5; i++) {
					apf[i].process(ibuf, ibuf);
				}

				var gainsL = [1.6    , -1.3    , 1.0, -(1/1.3), + (1/1.6), + 1.4    , + (1/1.4), + 1.8    , + (1/1.8)];
				var gainsR = [(1/1.6), +(1/1.3), 1.0, +1.3    , + 1.6    , + (1/1.4), + 1.4    , + (1/1.8), + 1.8];

				for (var i = 0; i < 9; i++) {
					rsf[i].process(ibuf, rbuf);
					var gainL = gainsL[i];
					var gainR = gainsR[i];
					for (var j = 0; j < length; j++) {
						obufL[j] += rbuf[j] * gainL;
						obufR[j] += rbuf[j] * gainR;
					}
				}
			}
		}

		/**
		 * @constructor
		 */
		function I3DL2Reverb(sfreq, params) {
			var room = params[0];
			var roomHf = params[1];
			var decayTime = params[2];
			var decayHfRatio = params[3];
			var reflections = params[4];
			var reflectionsDelay = params[5];
			var reverb = params[6];
			var reverbDelay = params[7];
			var density = params[8];
			var diffusion = params[9];
			var hfReference = params[10];

			function milibelToRatio(mb) {
				if (mb <= -10000.0) return 0.0;
				if (mb >= 0.0) return 1.0;
				return Math.pow(10.0, mb * 0.01 / 20.0);
			}

			var iflt = new Iir(calcHsf(sfreq, hfReference, 1.0, milibelToRatio(roomHf)));

			var pdly = new Dly(reflectionsDelay * sfreq);
			var rdly = [new Dly(reverbDelay * sfreq), new Dly(reverbDelay * sfreq)];

			var edlyTime = [5.43216, 8.45346, 12.4367, 21.5463, 34.3876];
			var egains = [-0.83216, 0.75346, -0.6367, -0.8763, 0.7876];
			var edly = Array(5);
			for (var i = 0; i < 5; i++) {
				edly[i] = new Dly(edlyTime[i] * sfreq / 1000);
			}
			egains[0] *= density * 0.01;
			egains[1] *= density * 0.01;

			var ldlyTime = [60.0, 71.9345, 86.7545, 95.945];
			var ldly = Array(4);
			for (var i = 0; i < 4; i++) {
				ldly[i] = new Dly(ldlyTime[i] * sfreq / 1000);
			}

			var apfdlyTime = [7.80, 1.31, 6.53, 3.75];
			var apf = Array(4);
			for (var i = 0; i < 4; i++) {
				apf[i] = new Apf(apfdlyTime[i]*sfreq/1000, 0.81*diffusion*0.01);
			}

			var hidumpCoef = [1.0 - 0.047148289, 1.0 - 0.129277567, 1.0 - 0.026615973, 1.0 - 0.144486692];
			var hidump = Array(4);
			for (var i = 0; i < 4; i++) {
				hidump[i] = new Iir((decayHfRatio < 1.0) ? 
					calcHsf(sfreq, hfReference, 0.8, hidumpCoef[i] * decayHfRatio) : 
					calcLsf(sfreq, hfReference, 0.8, hidumpCoef[i] / decayHfRatio))
			}

			var feedbackGain = 0.5 * Math.pow(1.0 / 1000.0, 0.1 / decayTime);
			var earlyLevel = milibelToRatio(room + reflections);
			var lateLevel = milibelToRatio(room + reverb);

			function earlyMatrix(buf0, buf1, gain) {
				var length = buf0.length;
				for (var i = 0; i < length; i++) {
					var s0 = buf0[i], s1 = buf1[i];
					buf0[i] = s0 + s1;
					buf1[i] = (s0 - s1) * gain;
				}
			}
			function lateMatrix(buf0, buf1, buf2, buf3) {
				var length = buf0.length;
				for (var i = 0; i < length; i++) {
					var s0 = buf0[i], s1 = buf1[i], s2 = buf2[i], s3 = buf3[i];
					var s4 = s0 + s1, s5 = s0 - s1, s6 = s2 + s3, s7 = s2 - s3;
					buf0[i] = s4 + s6; buf1[i] = s5 + s7; buf2[i] = s4 - s6; buf3[i] = s5 - s7;
				}
			}

			var s_ebuf = new Float32Array(PROCESS_UNIT);
			var s_lbuf = Array(4);
			for (var i = 0; i < 4; i++) {
				s_lbuf[i] = new Float32Array(PROCESS_UNIT);
			}

			this.process = function(ibuf, obufL, obufR) {
				var length = ibuf.length;
				iflt.process(ibuf, ibuf);
				pdly.process(ibuf, ibuf);
				for (var i = 0; i < PROCESS_UNIT; i++) {
					s_ebuf[i] = 0;
				}
				var ebuf = [ibuf, s_ebuf.subarray(0, length)];
				var lbuf = Array(4);
				for (var i = 0; i < 4; i++) {
					lbuf[i] = s_lbuf[i].subarray(0, length);
				}
				for (var i = 0; i < 5; i++) {
					earlyMatrix(ebuf[0], ebuf[1], egains[i]);
					edly[i].process(ebuf[1], ebuf[1]);
				}
				for (var i = 0; i < 2; i++) {
					rdly[i].process(ebuf[i], ebuf[i]);
				}
				for (var i = 0; i < 4; i++) {
					ldly[i].read(lbuf[i]);
				}
				lateMatrix(lbuf[0], lbuf[1], lbuf[2], lbuf[3]);
				for (var i = 0; i < 4; i++) {
					hidump[i].process(lbuf[i], lbuf[i]);
					apf[i].process(lbuf[i], lbuf[i]);
					for (var j = 0; j < length; j++) {
						lbuf[i][j] *= feedbackGain;
					}
				}
				var obuf = [obufL, obufR];
				for (var i = 0; i < 2; i++) {
					for (var j = 0; j < length; j++) {
						obuf[i][j] = lbuf[i][j] * lateLevel + ebuf[i][j] * earlyLevel;
						lbuf[i][j] += ebuf[i][j];
					}
				}
				for (var i = 0; i < 4; i++) {
					ldly[i].write(lbuf[i]);
				}
			}
		}
		function genIR(dspId, numChannels, numSamples, params) {
			var wactx = CriNc.wactx;
			var dsp;
			if (dspId == 0) {
				dsp = new Reverb(wactx.sampleRate, params);
			} else if (dspId == 1) {
				dsp = new I3DL2Reverb(wactx.sampleRate, params);
			} else {
				return null;
			}

			var ibuf = new Float32Array(numSamples);
			ibuf[0] = 1.0;
			var buffer = wactx.createBuffer(numChannels, numSamples, wactx.sampleRate);
			var obufL = buffer.getChannelData(0);
			var obufR = buffer.getChannelData(1);
			for (var i = 0; i < numSamples; i += PROCESS_UNIT) {
				var unit = Math.min(numSamples - i, PROCESS_UNIT);
				dsp.process(ibuf.subarray(i, i + unit),
					obufL.subarray(i, i + unit),
					obufR.subarray(i, i + unit));
			}
			return buffer;
		};
		/**
		 * リバーブDSP
		 * @constructor
		 */
		CriNc.FxReverb = function(wactx) {
			this.id = 0;
			this.bypass = false;

			this.verb = wactx.createConvolver();
			this.input = this.verb;
			this.output = this.verb;

			this.dirty = true;
			this.params = [
				3000,	// reverbTime
				10,		// roomSize
				100,	// predelayTime
				0,		// cofLow
				8000,	// cofHigh
			];
		};
		CriNc.FxReverb.prototype = {
			setParam: function(paramId, value) {
				if (this.params[paramId] != value) {
					this.dirty = true;
					this.params[paramId] = value;
				}
			},
			update: function() {
				if (this.dirty) {
					var params = this.params;
					var numSamples = ((params[0] + params[2]) * CriNc.wactx.sampleRate / 1000)|0;
					this.verb.buffer = genIR(0, 2, numSamples, params);
					this.dirty = false;
				}
			},
			destroy: function() {
				this.verb.disconnect();
			},
		};
		/**
		 * I3DL2リバーブDSP
		 * @constructor
		 */
		CriNc.FxI3DL2Reverb = function(wactx) {
			this.id = 2;
			this.bypass = false;

			this.verb = wactx.createConvolver();
			this.input = this.verb;
			this.output = this.verb;

			this.dirty = true;
			this.params = [
				-1000.0,	// room
				-100.0,		// roomHF
				1.49,		// decayTime
				0.83,		// decayHFRatio
				-2602.0,	// reflections
				0.007,		// reflectionsDelay
				200.0,		// reverb
				0.011,		// reverbDelay
				100.00,		// diffusion
				100.00,		// density
				5000.0,		// hfReference
			];
		};
		CriNc.FxI3DL2Reverb.prototype = {
			setParam: function(paramId, value) {
				if (this.params[paramId] != value) {
					this.dirty = true;
					this.params[paramId] = value;
				}
			},
			update: function() {
				if (this.dirty) {
					var params = this.params;
					var numSamples = ((params[5] + params[7] + params[2]) * CriNc.wactx.sampleRate)|0;
					this.verb.buffer = genIR(1, 2, numSamples, params);
					this.dirty = false;
				}
			},
			destroy: function() {
				this.verb.disconnect();
			},
		};
		/**
		 * エコーDSP
		 * @constructor
		 */
		CriNc.FxEcho = function(wactx) {
			this.id = 1;
			this.bypass = false;

			this.delay = wactx.createDelay(1);
			this.feedback = wactx.createGain();
			this.delay.connect(this.feedback);
			this.feedback.connect(this.delay);
			this.input = this.delay;
			this.output = this.delay;

			this.params = {
				delayTime: 300,
				feedback: 0.5,
			};
		};
		CriNc.FxEcho.prototype = {
			setParam: function(paramId, value) {
				switch (paramId) {
				case 0:this.params.delayTime = value; break;
				case 1:this.params.feedback = value; break;
				}
			},
			update: function() {
				setParam(this.delay.delayTime, Math.min(Math.max(this.params.delayTime * 0.001, 0.1), 1));
				setParam(this.feedback.gain, this.params.feedback);
			},
			destroy: function() {
				this.delay.disconnect();
				this.feedback.disconnect();
			},
		};
		/**
		 * ディレイDSP
		 * @constructor
		 */
		CriNc.FxDelay = function(wactx) {
			this.id = 3;
			this.bypass = false;

			this.delay = wactx.createDelay(1);
			this.input = this.delay;
			this.output = this.delay;

			this.params = {
				delayTime: 300,
			};
		};
		CriNc.FxDelay.prototype = {
			setParam: function(paramId, value) {
				switch (paramId) {
				case 0:this.params.delayTime = value; break;
				}
			},
			update: function() {
				setParam(this.delay.delayTime, Math.min(Math.max(this.params.delayTime * 0.001, 0.1), 1));
			},
			destroy: function() {
				this.delay.disconnect();
			},
		};
		/**
		 * バンドパスフィルタDSP
		 * @constructor
		 */
		CriNc.FxBandpass = function(wactx) {
			this.id = 4;
			this.bypass = false;

			this.filter = wactx.createBiquadFilter();
			this.input = this.filter;
			this.output = this.filter;

			this.params = {
				cofLow: 0,
				cofHigh: 24000,
			};
		};
		CriNc.FxBandpass.prototype = {
			setParam: function(paramId, value) {
				switch (paramId) {
				case 0:this.params.cofLow = value; break;
				case 1:this.params.cofHigh = value; break;
				}
			},
			update: function() {
				updateBpf(this.filter, this.params.cofLow, this.params.cofHigh);
			},
			destroy: function() {
				this.filter.disconnect();
			},
		};
		/**
		 * バイクワッドフィルタDSP
		 * @constructor
		 */
		CriNc.FxBiquad = function(wactx) {
			this.id = 5;
			this.bypass = false;

			this.filter = wactx.createBiquadFilter();
			this.input = this.filter;
			this.output = this.filter;

			this.params = {
				type: 0,
				freq: 8000,
				q: 1.0,
				gain: 1.0,
			};
		};
		CriNc.FxBiquad.prototype = {
			setParam: function(paramId, value) {
				switch (paramId) {
				case 0:this.params.type = value; break;
				case 1:this.params.freq = value; break;
				case 2:this.params.q = value; break;
				case 3:this.params.gain = value; break;
				}
			},
			update: function() {
				this.filter.type = bqfTypeTable[this.params.type];
				setParam(this.filter.frequency, this.params.freq);
				setParam(this.filter.Q, this.params.q);
				setParam(this.filter.gain, this.params.gain);
			},
			destroy: function() {
				this.filter.disconnect();
			},
		};
		/**
		 * 3バンドEQ DSP
		 * @constructor
		 */
		CriNc.Fx3BandEq = function(wactx) {
			this.id = 6;
			this.bypass = false;

			this.eq = [];
			for (var i = 0; i < 3; i++) {
				this.eq.push(wactx.createBiquadFilter());
			}
			for (var i = 0; i < this.eq.length - 1; i++) {
				this.eq[i].connect(this.eq[i + 1]);
			}
			this.input = this.eq[0];
			this.output = this.eq[this.eq.length - 1];

			// (type, freq, q, gain) * 3
			this.params = Array(4 * 3);
		};
		CriNc.Fx3BandEq.prototype = {
			setParam: function(paramId, value) {
				this.params[paramId] = value;
			},
			update: function() {
				var params = this.params;
				for (var i = 0; i < 3; i++) {
					var eq = this.eq[i];
					eq.type = bqfTypeTable[params[i*4]];
					setParam(eq.frequency, params[i*4+1]);
					setParam(eq.Q, params[i*4+2]);
					setParam(eq.gain, params[i*4+3]);
				}
			},
			destroy: function() {
				for (var i = 0; i < this.eq.length; i++) {
					this.eq[i].disconnect();
				}
			},
		};
		/**
		 * コンプレッサDSP
		 * @constructor
		 */
		CriNc.FxCompressor = function(wactx) {
			this.id = 7;
			this.bypass = false;

			this.comp = wactx.createDynamicsCompressor();
			this.gain = wactx.createGain();

			setParam(this.comp.knee, 0);

			this.input = this.comp;
			this.output = this.gain;
			this.comp.connect(this.gain);

			this.params = Array(9);
		};
		CriNc.FxCompressor.prototype = {
			setParam: function(paramId, value) {
				this.params[paramId] = value;
			},
			update: function() {
				var params = this.params;
				var comp = this.comp;
				var gain = this.gain;
				setParam(comp.threshold, 20 * Math.log10(params[0]));
				setParam(comp.ratio, Math.min(20, params[1]));
				setParam(comp.attack,  Math.min(1, params[2] * 0.001));
				setParam(comp.release, Math.min(1, params[3] * 0.001));
				setParam(gain.gain, params[4]);
			},
			destroy: function() {
				this.comp.disconnect();
				this.gain.disconnect();
			},
		};
		/**
		 * リミッターDSP
		 * @constructor
		 */
		CriNc.FxLimiter = function(wactx) {
			this.id = 8;
			this.bypass = false;

			this.comp = wactx.createDynamicsCompressor();
			this.gain = wactx.createGain();

			setValue(this.comp.knee, 0);
			setValue(this.comp.ratio, 20);

			this.input = this.comp;
			this.output = this.gain;
			this.comp.connect(this.gain);

			this.params = Array(6);
		};
		CriNc.FxLimiter.prototype = {
			setParam: function(paramId, value) {
				this.params[paramId] = value;
			},
			update: function() {
				var params = this.params;
				var comp = this.comp;
				var gain = this.gain;
				setParam(comp.threshold, 20 * Math.log10(params[0]));
				setParam(comp.attack,  Math.min(1, params[2] * 0.001));
				setParam(comp.release, Math.min(1, params[3] * 0.001));
				setParam(gain.gain, params[4]);
			},
			destroy: function() {
				this.comp.disconnect();
				this.gain.disconnect();
			},
		};
		/**
		 * WaveData
		 * @constructor
		 */
		CriNc.WaveData = function() {
			this.id = 0;
			this.buffer = null;
			this.url = null;
			this.error = false;
			this.offset = 0;
			this.duration = 0;
			this.loopEmbd = false;
			this.loopStart = 0;
			this.loopEnd = 0;
		};
		CriNc.Aux = function(numChannels) {
			this.buffers = [];
			this.cbfunc = null;
			this.cbobj = null;
			this.processId = null;
			this.worklet = null;
			this.numChannels = numChannels;

			this.process = function (numSamples){
				var stack = stackSave();
				var buffers = stackAlloc(this.numChannels * 4);
				for(var i=0; i < this.numChannels; i++){
					var buffer = stackAlloc(numSamples * 4);
					HEAPU32[(buffers >> 2) + i] = buffer;
				}

				//aux.c からバッファーを取ってきて自分に入れる
				if(this.cbfunc){
					{{{makeDynCall("vii", 'waveData.cbfunc')}}}(waveData.cbobj, 1);
				}

				var float_buffer = [];
				for(var i=0; i < this.numChannels; i++){
					var buffer = HEAPU32[(buffers >> 2) + i];
					float_buffer[i] = new Float32Array(HEAPF32.subarray((buffer) >> 2, (buffer >> 2) + numSamples));
				}

				//workletにメッセージを送る
				this.worklet.port.postMessage({
					"type":"AudioFrame", "data":float_buffer
				}, [float_buffer[0].buffer]);

				stackRestore(stack);
			}.bind(this);
		};
		CriNc.Aux.prototype = {
			start: function(){
				this.worklet.port.onmessage = function(cmd) {
					this.process(cmd.data.data);
				}.bind(this);
			},
			cleanup: function(){
				this.worklet = null;
				this.cbfunc = null;
				this.cbobj = null;
			},
			setCallback: function(cbfunc, cbobj){
				this.cbfunc = cbfunc;
				this.cbobj = cbobj;
			},
		};
	},
	unlockWebAudio: function() {
		if (!CriNc.unlocked) {
			// モバイル環境では1音鳴らすことでサスペンドを解除する
			if (CriNc.os == "iOS" || CriNc.os == "android") {
				var source = CriNc.wactx.createBufferSource();
				source.connect(CriNc.wactx.destination);
				source.start(0);
				setTimeout(function() {
					source.disconnect();
					source = null;
				}, 100);
			}
			// Autoplay Policyによるサスペンドを解除する
			if (!CriNc.suspended && CriNc.wactx.state === "suspended") {
				CriNc.wactx.resume();
			}
			CriNc.unlocked = true;
		}
		// イベントを解除
		CriNc.unregisterEvents();
	},
	registerEvents: function() {
		if (CriNc.os == "iOS") {
			window.addEventListener("touchstart", CriNc.unlockWebAudio);
		} else {
			window.addEventListener("touchend", CriNc.unlockWebAudio);
			window.addEventListener("click", CriNc.unlockWebAudio);
			window.addEventListener("keydown", CriNc.unlockWebAudio);
		}
		window.addEventListener("unload", CriNc.unregisterEvents);
	},
	unregisterEvents: function() {
		if (CriNc.os == "iOS") {
			window.addEventListener("touchstart", CriNc.unlockWebAudio);
		} else {
			window.removeEventListener("touchend", CriNc.unlockWebAudio);
			window.removeEventListener("click", CriNc.unlockWebAudio);
			window.removeEventListener("keydown", CriNc.unlockWebAudio);
		}
		window.removeEventListener("unload", CriNc.unregisterEvents);
	},
	initWorker: function() {
		CriNc.worker = new Worker("criadx2.worker.js");
		//CriNc.worker = new Worker("../../src/adx2.worker.js");
		CriNc.worker.onmessage = CriNc.onWorkerMessage;
		CriNc.worker.onerror = CriNc.onWorkerError;
	},
	onWorkerMessage: function(e) {
		var cmd = e.data;
		if (cmd.type === "decoded") {
			var waveData = CriNc.buffers[cmd["id"]];
			var data = cmd["data"];
			var numChannels = cmd["numChannels"];
			var samplingRate = cmd["samplingRate"];
			var loopStart = cmd["loopStart"];
			var loopEnd = cmd["loopEnd"];
			waveData.buffer = CriNc.wactx.createBuffer(numChannels, data[0].length, samplingRate);
			for (var ch = 0; ch < numChannels; ch++) {
				waveData.buffer.copyToChannel(data[ch], ch);
			}
			// 音声の長さの計算
			waveData.originalSampleRate = samplingRate;
			waveData.originalSamples = data[0].length;
			waveData.duration = +waveData.originalSamples / samplingRate;
			// ループ設定
			if (loopStart < loopEnd) {
				waveData.loopEmbd = true;
				waveData.loopStart = loopStart / samplingRate;
				waveData.loopEnd = loopEnd / samplingRate;
			} else {
				waveData.loopStart = 0;
				waveData.loopEnd = waveData.duration;
			}
			if (waveData.cbfunc) {
				{{{makeDynCall("vii", 'waveData.cbfunc')}}}(waveData.cbobj, 1);
			}
		} else if (cmd.type === "decode-error") {
			console.error("Failed to decode audio data. error:" + cmd.error);
			var waveData = CriNc.buffers[cmd.id];
			waveData.error = true;
			if (waveData.cbfunc) {
				{{{makeDynCall("vii", 'waveData.cbfunc')}}}(waveData.cbobj, 0);
			}
		}
	},
	onWorkerError: function(e) {
		console.error("Failed to start ADX2 Worker.");
		CriNc.workerError = true;
		for (var i in CriNc.buffers) {
			var waveData = CriNc.buffers[i];
			waveData.error = true;
			if (waveData.cbfunc) {
				{{{makeDynCall("vii", 'waveData.cbfunc')}}}(waveData.cbobj, 0);
			}
		}
	},
},
WAJS_Initialize: function() {
	var itf = CriNc.itf = Module["CriNcItf"] = Module["CriNcItf"] || {};
	var ua = navigator.userAgent.toLowerCase();
	// ブラウザ判定 ---------------------------------
	if (ua.match(/msie|trident/)) {
		CriNc.browser = 'msie';
	} else if (ua.match(/edge/)) {
		CriNc.browser = 'edge';
	} else if (ua.match(/vivaldi/)) {
		CriNc.browser = 'vivaldi';
	} else if (ua.match(/opera/)) {
		CriNc.browser = 'opera';
	} else if (ua.match(/chrome/)) {
		CriNc.browser = 'chrome';
	} else if (ua.match(/firefox/)) {
		CriNc.browser = 'firefox';
	} else if (ua.match(/safari/)) {
		CriNc.browser = 'safari';
	} else {
		CriNc.browser = 'n/a';
	}

	// OS判定 ---------------------------------
	if (ua.match(/iphone|ipad/)) {
		CriNc.os = "iOS";
	} else if (ua.match(/android/)) {
		CriNc.os = "android";
	} else if (ua.match(/win/)) {
		CriNc.os = "windows";
	} else if (ua.match(/mac/)) {
		CriNc.os = "macOS";
	} else if (ua.match(/linux/)) {
		CriNc.os = "linux";
	} else {
		CriNc.os = "n/a";
	}
	//console.log(CriNc.os + ", " + CriNc.browser);
	var AudioContext = window.AudioContext || window.webkitAudioContext;
	if (AudioContext) {
		CriNc.setupWebAudio();
	} else {
		console.warn("Web Audio API is not supported.");
		CriNc.setupHTML5Audio();
	}

	// AudioContextを作成
	if (AudioContext) {
		var context = CriNc.wactx || itf["audioContext"] || new AudioContext();

		CriNc.wactx = itf["audioContext"] = context;

		// バスを8個作成する
		for (var i = 0; i < 8; i++) {
			CriNc.buses.push(new CriNc.Bus(context, i === 0));
		}
		itf["audioBuses"] = CriNc.buses;

		CriNc.unlocked = false;
		// Autoplay Policy対応のためのイベント登録
		//CriNc.registerEvents();
		// ユーザー呼び出し用にAPIを登録
		itf["unlockWebAudio"] = CriNc.unlockWebAudio;
	}
},
JSAUX_Initialize: function() {
	const workletcode = `
	class AuxAudioWorkletProcessor extends AudioWorkletProcessor {
		constructor(options) {
			super();

			this.buffers = [];
			this.channels = options.outputChannelCount[0];

			this.port.onmessage = function(cmd) {
				switch (cmd.data.type) {
					case "AudioFrame":
						this.buffers.push(cmd.data.data);
						break;
				}
			}.bind(this);
		}
		process(inputs, outputs, parameters) {
			var output = outputs[0];
			this.port.postMessage({
				id:this.id,
				type : "DataCallback",
				data : output[0].length
			});

			var data = this.buffers.shift();

			for (var i = 0; i < this.channels; i++) {
				var dst = output[i];
				if (data) {
					dst.set(data[i]);
				} else {
					dst.fill(0.0);
				}
			}
			return true;
		}
	}
	registerProcessor('AuxAudioWorklet', AuxAudioWorkletProcessor); `;
	Module["CriNcItf"]["audioContext"].audioWorklet.addModule('data:text/javascript,' + encodeURI(workletcode));
},
WAJS_Finalize: function() {
	// 終了処理
	CriNc.voices.length = 1;
	CriNc.buffers = {};
	for (var i in CriNc.buses) {
		var bus = CriNc.buses[i];
		bus.detachAllFx();
		bus.resetRouting();
	}
	CriNc.buses.length = 0;
	
	var itf = CriNc.itf;
	delete itf["audioBuses"];
	delete itf["audioContext"];
},
WAJS_ExecuteMain: function() {
	if (CriNc.updateAnalyzers) {
		CriNc.updateAnalyzers();
	}
},
WAJS_SuspendContext: function() {
	CriNc.suspended = true;
	CriNc.applySuspendState();
	return true;
},
WAJS_ResumeContext: function() {
	CriNc.suspended = false;
	CriNc.applySuspendState();
	return true;
},
WAJS_AttachDspFx: function(busId, fxId) {
	if (!CriNc.wactx) return;
	//console.log("WAJS_AttachDspFx", busId, fxId);
	var bus = CriNc.buses[busId];
	if (!bus) return;
	var fx = null;
	switch (fxId) {
	case  0: fx = new CriNc.FxReverb(CriNc.wactx); break;
	case  1: fx = new CriNc.FxEcho(CriNc.wactx); break;
	case  2: fx = new CriNc.FxI3DL2Reverb(CriNc.wactx); break;
	case  3: fx = new CriNc.FxDelay(CriNc.wactx); break;
	case  4: fx = new CriNc.FxBandpass(CriNc.wactx); break;
	case  5: fx = new CriNc.FxBiquad(CriNc.wactx); break;
	case  6: fx = new CriNc.Fx3BandEq(CriNc.wactx); break;
	case  7: fx = new CriNc.FxCompressor(CriNc.wactx); break;
	case  8: fx = new CriNc.FxLimiter(CriNc.wactx); break;
	}
	if (fx) bus.attachFx(fx);
},
WAJS_ResetDspBus: function(busId) {
	if (!CriNc.wactx) return;
	var bus = CriNc.buses[busId];
	bus.detachAllFx();
	bus.resetRouting();
},
WAJS_UpdateDspBus: function(busId) {
	if (!CriNc.wactx) return;
	var bus = CriNc.buses[busId];
	bus.update();
},
WAJS_SetDspBusVolume: function(busId, volume) {
	if (!CriNc.wactx) return;
	var bus = CriNc.buses[busId];
	bus.setVolume(volume);
},
WAJS_SetDspBusSendLevel: function(busId, target, level) {
	if (!CriNc.wactx) return;
	var bus = CriNc.buses[busId];
	bus.setRouting(target, level);
},
WAJS_SetDspFxParam: function(busId, fxId, paramId, value) {
	if (!CriNc.wactx) return;
	var bus = CriNc.buses[busId];
	var fx = bus.findFx(fxId);
	if (fx) {
		fx.setParam(paramId, value);
	}
},
WAJS_UpdateDspFx: function(busId, fxId) {
	if (!CriNc.wactx) return;
	var bus = CriNc.buses[busId];
	var fx = bus.findFx(fxId);
	if (fx) {
		fx.update();
	}
},
WAJS_SetDspFxBypass: function(busId, fxId, bypass) {
	if (!CriNc.wactx) return;
	var bus = CriNc.buses[busId];
	var fx = bus.findFx(fxId);
	if (fx && fx.bypass != bypass) {
		fx.bypass = bypass;
		bus.update();
	}
},
WAJS_CreateAnalyzer: function(interval, peakholdTime) {
	if (!CriNc.wactx) return 0;
	var analyzer = new CriNc.Analyzer(interval, peakholdTime);
	var nodeId = CriNc.extnodes.indexOf(null, 1);
	if (nodeId < 0) {
		nodeId = CriNc.extnodes.push(analyzer) - 1;
	} else {
		CriNc.extnodes[nodeId] = analyzer;
	}
	CriNc.analyzers.push(analyzer);
	return nodeId;

},
WAJS_DestroyAnalyzer: function(nodeId) {
	if (!CriNc.wactx) return;
	var analyzer = CriNc.extnodes[nodeId];
	if (analyzer) {
		CriNc.extnodes[nodeId] = null;
		var index = CriNc.analyzers.indexOf(analyzer);
		if (index >= 0) {
			CriNc.analyzers.splice(index, 1);
		}
	}
},
WAJS_GetAnalyzedLevel: function(nodeId, rms, peak, peakhold) {
	if (CriNc.wactx) {
		var analyzer = CriNc.extnodes[nodeId];
		if (analyzer) {
			HEAPF32[rms>>2] = analyzer.rmsLevel;
			HEAPF32[peak>>2] = analyzer.peakLevel;
			HEAPF32[peakhold>>2] = analyzer.peakholdLevel;
			return 1;
		}
	}
	return 0;
},
WAJS_GetAnalyzedFrequencyData: function(nodeId, data) {
	if (!CriNc.wactx) return;
	var analyzer = CriNc.extnodes[nodeId];
	if (analyzer) {
		analyzer.getFrequencyData(HEAPF32.subarray(data>>2, (data>>2) + analyzer.node.frequencyBinCount));
		return 1;
	}
	return 0;
},
WAJS_GetAnalyzedTimeDomainData: function(nodeId, data) {
	if (!CriNc.wactx) return;
	var analyzer = CriNc.extnodes[nodeId];
	if (analyzer) {
		analyzer.getTimeDomainData(HEAPF32.subarray(data>>2, (data>>2) + analyzer.node.fftSize));
		return 1;
	}
	return 0;
},
WAJS_GetAudioTime: function(srate) {
	if (CriNc.wactx) {
		HEAP32[srate>>2] = CriNc.wactx.sampleRate;
		return CriNc.wactx.currentTime;
	}
	return 0;
},
WAJS_AttachBusFilterNode: function(busId, nodeId) {
	if (!CriNc.wactx) return;
	var bus = CriNc.buses[busId];
	var node = CriNc.extnodes[nodeId];
	if (bus && node) {
		bus.attachFilter(node);
	}
},
WAJS_DetachBusFilterNode: function(busId, nodeId) {
	if (!CriNc.wactx) return;
	var bus = CriNc.buses[busId];
	var node = CriNc.extnodes[nodeId];
	if (bus && node) {
		bus.detachFilter(node);
	}
},
WAJS_CreateBank: function(numContents) {
	var bankId = ++CriNc.bankCount;
	CriNc.banks[bankId] = [];
	return bankId;
},
WAJS_DestroyBank: function(bankId) {
	var buffersInBank = CriNc.banks[bankId];
	for (var i = 0; i < buffersInBank.length; i++) {
		var bufferId = buffersInBank[i].id;
		var buffer = CriNc.buffers[bufferId];
		if (buffer.url) URL.revokeObjectURL(buffer.url);
		delete CriNc.buffers[buffer.id];
		delete CriNc.preloaded[buffer.ptr];
	}
	delete CriNc.banks[bankId];
},
WAJS_LoadData: function(bankId, ptr, size, 
	originalSampleRate, originalSamples, encoderDelay, 
	loopStart, loopEnd, cbfunc, cbobj) {

	//console.log("WAJS_LoadData: ", ptr);

	// 一旦ヒープメモリからArrayBufferコピーする必要がある
	var data = HEAPU8.buffer.slice(ptr, ptr + size);

	// WaveDataを作成
	var waveData = new CriNc.WaveData();
	waveData.originalSampleRate = originalSampleRate;
	waveData.originalSamples = originalSamples;

	var bufferId = ++CriNc.bufferCount;
	waveData.id = bufferId;
	waveData.ptr = ptr;
	CriNc.buffers[bufferId] = waveData;

	if (bankId > 0) {
		CriNc.banks[bankId].push(waveData);
	}

	var u8data = new Uint8Array(data);
	if (u8data[0] === 72 && u8data[1] === 67 && u8data[2] === 65 && u8data[3] === 0) {
		// HCAフォーマットはWorkerデコードを行う
		if (!CriNc.worker && !CriNc.workerError) {
			CriNc.initWorker();
		}
		waveData.cbfunc = cbfunc;
		waveData.cbobj = cbobj;
		if (CriNc.worker) {
			CriNc.worker.postMessage({type:"decode",id:bufferId,data:data});
		} else {
			waveData.error = true;
			if (cbfunc) {
				{{{makeDynCall("vii", 'cbfunc')}}}(cbobj, 0);
			}
		}
	} else {
		// AACフォーマットはWebAudioのデコーダを使用する
		if (CriNc.browser == "safari") {
			// 既にディレイ分を全てカットされている
			encoderDelay = 0;
		}

		if (CriNc.browser == "firefox" && CriNc.os == "macOS") {
			if (encoderDelay == 5186) {		// HE-AACの時間
				// HE-AACは既にディレイ分を2112*2だけカットされている
				encoderDelay = 962;
			}
		}

		// オフセット計算
		waveData.offset = +encoderDelay / originalSampleRate;

		// ループ設定
		if (loopStart < loopEnd) {
			waveData.loopEmbd = true;
			waveData.loopStart = +(encoderDelay + loopStart) / originalSampleRate;
			waveData.loopEnd = +(encoderDelay + loopEnd) / originalSampleRate;
		} else {
			waveData.loopStart = +(encoderDelay) / originalSampleRate;
			waveData.loopEnd = +(encoderDelay + originalSamples) / originalSampleRate;
		}
		// 音声の長さの計算
		waveData.duration = +originalSamples / originalSampleRate;

		if (CriNc.wactx) {
			// WebAudioでデコード
			CriNc.wactx.decodeAudioData(data, function(buffer) {
				waveData.buffer = buffer;
				if (originalSamples == 0) {
					waveData.offset = 0;
					waveData.duration = buffer.duration;
				}
				//console.log(buffer);
				if (cbfunc) {
					{{{makeDynCall("vii", 'cbfunc')}}}(cbobj, 1);
				}
			}, function() {
				waveData.error = true;
				if (cbfunc) {
					{{{makeDynCall("vii", 'cbfunc')}}}(cbobj, 0);
				}
			});
		} else {
			// Audioで再生するためにBlobURLでロード
			var blob = new Blob([data]);
			waveData.url = URL.createObjectURL(blob);
			if (cbfunc) {
				{{{makeDynCall("vii", 'cbfunc')}}}(cbobj, 1);
			}
			//console.log(waveData.url);
		}
	}

	return bufferId;
},
WAJS_ReleaseData: function(bufferId) {
	//console.log("WAJS_ReleaseData: ", bufferId);
	var waveData = CriNc.buffers[bufferId];
	if (waveData) {
		var buffer = CriNc.buffers[bufferId];
		if (buffer.url) URL.revokeObjectURL(buffer.url);
		delete CriNc.buffers[bufferId];
	}
},
WAJS_PreloadData: function(bankId, ptr, size,
	originalSampleRate, originalSamples, encoderDelay,
	loopStart, loopEnd, cbfunc, cbobj) {
	var id = _WAJS_LoadData(bankId, ptr, size,
		originalSampleRate, originalSamples, encoderDelay,
		loopStart, loopEnd, cbfunc, cbobj);
	CriNc.preloaded[ptr] = CriNc.buffers[id];
	return id;
},
WAJS_GetPreloadBuffer: function(ptr) {
	var waveData = CriNc.preloaded[ptr];
	return (waveData) ? waveData.id : 0;
},
WAJS_GetBufferStatus: function(bufferId) {
	var waveData = CriNc.buffers[bufferId];
	if (waveData) {
		if (!CriNc.wactx) {
			return 1;
		}
		if (waveData.buffer) {
			return 1;		// Complete
		} else if (waveData.error) {
			return 2;		// Error
		} else {
			return 0;		// Loading
		}
	}
	return -1;
},
WAJS_GetBufferFormat: function(bufferId, srate, nch, nsmpl) {
	var waveData = CriNc.buffers[bufferId];
	if (waveData) {
		if (!CriNc.wactx) {
			HEAP32[srate >> 2] = waveData.originalSampleRate;
			HEAP32[nch >> 2] = 2;
			HEAP32[nsmpl >> 2] = waveData.originalSamples;
			return true;
		}
		if (waveData.buffer) {
			HEAP32[srate >> 2] = waveData.originalSampleRate;
			HEAP32[nch >> 2] = waveData.buffer.numberOfChannels;
			HEAP32[nsmpl >> 2] = waveData.originalSamples;
			return true;
		}
	}
	return false;
},
WAJS_CreateVoice: function(maxChannels) {
	var voice = new CriNc.Voice(CriNc.wactx, maxChannels, 2);
	// 空きスロットを探す
	var id = CriNc.voices.indexOf(null, 1);
	if (id >= 0) {
		// 開いていたらそこにセットする
		CriNc.voices[id] = voice;
	} else {
		// 開いてなければ配列に追加する
		id = CriNc.voices.push(voice) - 1;
	}
	return id;
},
WAJS_DestroyVoice: function(id) {
	CriNc.voices[id] = null;
},
WAJS_SetDecoder: function(id, decoder) {
	//if (!CriNc.wactx) return;
	CriNc.voices[id].decoder = decoder;
},
WAJS_GetDecoder: function(id) {
	//if (!CriNc.wactx) return;
	return CriNc.voices[id].decoder;
},
WAJS_SetData: function(id, bufferId) {
	//if (!CriNc.wactx) return;
	CriNc.voices[id].setData(CriNc.buffers[bufferId]);
},
WAJS_Setup: function(id, startTime, loopLimit, filterNodeId) {
	//if (!CriNc.wactx) return;
	CriNc.voices[id].setup(startTime * 0.001, // ミリ秒から秒に
		loopLimit, (filterNodeId) ? CriNc.extnodes[filterNodeId] : null);
},
WAJS_SetSamplingRate: function(id, rate) {
	//if (!CriNc.wactx) return;
	CriNc.voices[id].setSamplingRate(rate);
},
WAJS_Start: function(id, type) {
	//if (!CriNc.wactx) return;
	CriNc.voices[id].start(type);
},
WAJS_Stop: function(id) {
	//if (!CriNc.wactx) return;
	CriNc.voices[id].stop();
},
WAJS_IsPlaying: function(id) {
	//if (!CriNc.wactx) return;
	return CriNc.voices[id].playing;
},
WAJS_Update: function(id) {
	//if (!CriNc.wactx) return;
	CriNc.voices[id].update();
},
WAJS_Pause: function(id, paused) {
	//if (!CriNc.wactx) return;
	CriNc.voices[id].pause(paused);
},
WAJS_PutPacket: function(id, packet) {
	//if (!CriNc.wactx) return;
	return CriNc.voices[id].putPacket(packet);
},
WAJS_GetTime: function(id, count, tunit) {
	//if (!CriNc.wactx) return;
	CriNc.voices[id].getTime(count, tunit);
},
WAJS_SetOutputMatrix: function(id, nch, nspk, matrix) {
	//if (!CriNc.wactx) return;
	CriNc.voices[id].setOutputMatrix(nch, nspk, matrix);
},
WAJS_SetPreDelay: function(id, time) {
	//if (!CriNc.wactx) return;
	CriNc.voices[id].setPreDelay(time * 0.001);
},
WAJS_SetEnvelopeActive: function(id, active) {
	//if (!CriNc.wactx) return;
	CriNc.voices[id].setEnvActive(active);
},
WAJS_SetEnvelopeParam: function(id, paramId, value) {
	//if (!CriNc.wactx) return;
	CriNc.voices[id].setEnvParam(paramId, value);
},
WAJS_SetBiquadActive: function(id, active) {
	//if (!CriNc.wactx) return;
	CriNc.voices[id].setBiqActive(active);
},
WAJS_SetBiquadType: function(id, value) {
	//if (!CriNc.wactx) return;
	CriNc.voices[id].setBiqType(value);
},
WAJS_SetBiquadFreq: function(id, value) {
	//if (!CriNc.wactx) return;
	CriNc.voices[id].setBiqFreq(value);
},
WAJS_SetBiquadQ: function(id, value) {
	//if (!CriNc.wactx) return;
	CriNc.voices[id].setBiqQ(value);
},
WAJS_SetBiquadGain: function(id, value) {
	//if (!CriNc.wactx) return;
	CriNc.voices[id].setBiqGain(value);
},
WAJS_UpdateBiquad: function(id) {
	//if (!CriNc.wactx) return;
	CriNc.voices[id].updateBiq();
},
WAJS_SetBandpassActive: function(id, active) {
	//if (!CriNc.wactx) return;
	CriNc.voices[id].setBpfActive(active);
},
WAJS_SetBandpassCofLo: function(id, value) {
	//if (!CriNc.wactx) return;
	CriNc.voices[id].setBpfCofLo(value);
},
WAJS_SetBandpassCofHi: function(id, value) {
	//if (!CriNc.wactx) return;
	CriNc.voices[id].setBpfCofHi(value);
},
WAJS_UpdateBandpass: function(id) {
	//if (!CriNc.wactx) return;
	CriNc.voices[id].updateBpf();
},
WAJS_ResetDspParameters: function(id) {
	//if (!CriNc.wactx) return;
	CriNc.voices[id].resetDspParams();
},
WAJS_SetRouting: function(id, busId, level) {
	//if (!CriNc.wactx) return;
	CriNc.voices[id].setRouting(busId, level);
},
WAJS_ResetRouting: function(id) {
	//if (!CriNc.wactx) return;
	CriNc.voices[id].resetRouting();
},
WAJS_SetCallback: function(id, cbfunc, cbobj) {
	if(CriNc.voices[id].aux){
		CriNc.voices[id].aux.setCallback(cbfunc, cbobj);
	}
}
};

autoAddDeps(LibraryCriNc, '$CriNc');
mergeInto(LibraryManager.library, LibraryCriNc);
