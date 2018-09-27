// Type definitions for athenajs 0.1.1
// Project: athenajs
// Definitions by: Nicolas Ramz https://github.warpdesign.fr

declare module 'athenajs' {
    export function Dom(sel?: string | HTMLElement): _Dom;
    export class Scene{
        constructor(options?: SceneOptions);
        map: Map;
        hudScene: Scene | null;
        running: boolean;
        addObject(object: Drawable): Scene;
        addObject(array: Array<Drawable>): Scene;
        animate(fxName: string, options: EffectOptions): Promise;
        bindEvents(eventList: String): void;
        fadeIn(duration: number): Promise;
        fadeOut(duration: number): Promise;
        fadeInAndOut(inDuration: number, delay: number, outDuration: number): Promise;
        getOpacity(): number;
        loadAudio(src: string, id?: string | null): void;
        loadImage(src: string, id: string): void;
        notify(name: string, data?: JSObject): void;
        setBackgroundImage(image:String|HTMLImageElement): void;
        setLayerPriority(layer: number, background: boolean): void;
        setMap(map: Map | Object, x?: number, y?: number): void;
        setOpacity(opacity: number): void;
        start(): void;
        stop(): void;
    }
    export class Game {
        constructor(options: GameOptions);
        setScene(scene: Scene): void;
        toggleSound(bool: boolean): void;
        scene: Scene;
        sound: boolean;
    }
    export class Drawable{
        constructor(type: string, options: DrawableOptions);
        addChild(child:Drawable): void;
        animate(name: string, options: object): Promise;
        center(): Drawable;
        destroy(data?:any): void;
        moveTo(x: number, y: number, duration?: number): Drawable;
        notify(id: string, data: object): void;
        onCollision?(object: Drawable): void;
        onEvent?(eventType: string, data?: JSObject): void;
        playSound(id: string, options?: { pan?: boolean, loop?: false }): void;
        setBehavior(behavior: string | Behavior, options?: JSObject): void;
        setScale(scale: number): void;
        getCurrentWidth(): number;
        getCurrentHeight(): number;
        getProperty<T>(prop: string): T;
        setProperty<T>(prop: string, value: T): void;
        setMask(mask: MaskOptions | null, exclude: boolean): void;
        stopAnimate(endValue?: number): void;
        reset(): void;
        show(): void;
        hide(): void;
        type: string;
        width: number;
        height: number;
        x:number;
        y:number;
        vx:number;
        vy: number;
        canCollide: boolean;
        currentMovement: string;
        running: boolean;
        movable: boolean;
        behavior: Behavior;
        currentMap: Map;
        data: any;
        visible: boolean;
    }

    export interface MaskOptions{
        x: number,
        y: number,
        width: number,
        height: number
    }

    export interface MenuItem {
        text: string,
        selectable: boolean,
        visible: boolean,
        active?: boolean
    }

    export interface MenuOptions {
        title: string,
        color: string,
        menuItems: MenuItem[]
    }

    export class Menu extends Drawable{
        constructor(id: string, options: MenuOptions);
        nextItem(): void;
        getSelectedItemIndex(): number;
    }

    export class SimpleText extends Drawable{
        constructor(type: string, simpleTextOptions: SimpleTextOptions);
        setText(text: string): void;
    }
    export class Paint extends Drawable{
        constructor(type: string, paintOptions: PaintOptions);
        arc(cx: number, cy: number, r: number, starteAngle: number, endAngle: number, fillStyle: string, borderSize: number): void;
        circle(cx: number, cy: number, r: number, fillStyle: string): void;
        circle(cx: number, cy: number, r: number, fillStyle: string, borderWidth?: number, borderStyle?: string): void;
        rect(x:number, y:number, width:number, height:number, color:string): void;
        name: string;
    }

    export class BitmapText extends Drawable{
        constructor(type: string, textOptions: BitmapTextOptions);
        setText(text: string): void;
    }

    export class Sprite extends Drawable {
        constructor(type: string, spriteOptions: SpriteOptions);
        addAnimation(name: string, imgPath: string, options: AnimOptions): void;
        setAnimation(name: string, fn?: Callback, frameNum?: number, revert?: boolean): void;
        clearMove(): void;
    }

    interface pixelPos {
        x: number,
        y: number
    }

    export class Map{
        constructor(options:MapOptions)
        addObject(obj: Drawable, layerIndex?: number): void;
        addTileSet(tiles: Tile[]): void;
        checkMatrixForCollision(buffer: number[], matrixWidth: number, x: number, y: number, behavior: number): boolean;
        clear(tileNum?: number, behavior?: number): void;
        getTileBehaviorAtIndex(col:number, row:number): number;
        getTileIndexFromPixel(x: number, y: number): pixelPos;
        moveTo(x: number, y: number): void;
        respawn(): void;
        setData(map: Uint8Array, behaviors: Uint8Array): void;
        setEasing(easing: string): void;
        shift(startLine:number, height:number): void;
        updateTile(col: number, row: number, tileNum?: number, behavior?: number): void;
        duration: number;
        numRows: number;
        numCols: number;
        width: number;
        height: number;
        tileWidth: number;
        tileHeight: number;
    }

    export class Tile{
        static TYPE: {
            AIR: 1,
            WALL: 2,
            LADDER: 3
        };
    }

    interface MapOptions{
        src: string,
        tileWidth: number,
        tileHeight: number,
        width: number,
        height: number,
        viewportW?: number,
        viewportH?: number,
        buffer?:ArrayBuffer
    }

    interface FXInstance{
        addFX(fxName: string, FxClass: { new(options: EffectOptions, display: Display): Effect }): void;
    }

    export const FX: FXInstance;

    export class _FX {
        /**
         * Creates the FX class, adding the linear easing
         */
        constructor();

        /**
         * Add a new Effect
         * @param {String} fxName The name of the effect to add.
         * @param {Effect} FxClass The Effect Class to add.
         */
        addFX(fxName: string, FxClass: { new(): Effect }): void;

        /**
         * Retrieve an effect Class by its name
         *
         * @param {String} fxName The name of the Effect to retrive.
         * @returns {Effect} the effect Class or undefined
         */
        getEffect(fxName: string): Effect;

        /**
         * Add a new easing function for other objects to use
         *
         * @param {String} easingName The name of the easing.
         * @param {Function} easingFn The function to be used for easing. This function may use these parameters: `x , t, b, c, d`
        */
        addEasing(easingName: string, easingFn: (x?: number, t?: number, b?: number, c?: number, d?: number) => void):void;

        /**
         * Retrieves an easing function
         *
         * @param {String} easingName The name of the easing function to retrive.
         * @returns {Function} The easing function, or linear function if it didn't exist.
         */
        getEasing(easingName: string): (x?: number, t?: number, b?: number, c?: number, d?: number) => void;
    }

    interface EffectOptions {

    }

    export class Effect {
        width: number;
        height: number;
        buffer: RenderingContext;
        animProgress: number;
        startValue: number;
        ended:boolean;
        /**
         * This the class constructor. Default options are:
         *
         * @param {Object} options
         * @param {Number} options.startValue The start value of the effect.
         * @param {Number} options.endValue The end value of the effect.
         * @param {Number} options.duration The duration of the effect (ms).*
         * @param {boolean} options.loop Set to true to make the effect loop.
         * @param {Display} display Reference to the Display in case a buffer is needed.
         */
        constructor(options: EffectOptions, display: Display);

        /**
         * Changes the easing function used for the ffect
         *
         * @param {Function} easing The new easing function.
         */
        setEasing(easing: (x?: number, t?: number, b?: number, c?: number, d?: number) => void): void;

        /**
         * Called when the ffect is started.
         *
         * This method can be overriden but the super should always be called
         */
        start():Promise;

        /**
         * called when the effect is stopped
         */
        stop(object: any, setEndValue: any): void;

        /**
         * Calculates current animation process
         *
         * This method can be overridden but the super should always be calle first
         */
        process(ctx: RenderingContext, fxCtx?: RenderingContext, obj?: any): boolean;
    }

    // why do we need this ?
    type RenderingContext = CanvasRenderingContext2D;

    interface DisplayOptions {
        width: number,
        height: number,
        type: string,
        layers?: boolean[],
        name: string
    }

    export class Display {
        /**
         * Creates a new Display instance
         *
         * @param {Object} options
         * @param {Number} [options.width=1024] The width of the display.
         * @param {Number} [options.height=768] The height of the display.
         * @param {String} [options.type] What type of rendere to use, only '2d' supported for now.
         * @param {Array<boolean>} [options.layers] An array describing each layer that will be added: [true, true] will create two background layers, set to true for a foreground layer.
         * @param {String} options.name The name of the display.
         * @param {(String|HTMLElement)} target The target where the game DOM element should be appended.
         */
        constructor(options: DisplayOptions, target: string | HTMLElement);

        /**
         * Creates a new (offscreen) drawing buffer
         *
         * @param {Number} width width of the buffer
         * @param {Number} height height of the buffer
         */
        getBuffer(width: number, height: number): RenderingContext;

        /**
         * Adds cross-browser event listener for the fullscreenchange event
         */
        // _addFullscreenEvents() {
        //     const target = this.target;

        //     target.addEventListener('webkitfullscreenchange', this._onFullscreenChange.bind(this), false);
        //     // Firefox doesn't seem to send this event on current fullscreen element
        //     document.addEventListener('mozfullscreenchange', this._onFullscreenChange.bind(this), false);
        //     target.addEventListener('fullscreenchange', this._onFullscreenChange.bind(this), false);
        //     target.addEventListener('MSFullscreenChange', this._onFullscreenChange.bind(this), false);
        // }

        /**
         * Handler called when `fullscreenchange` event is triggered by the browser
         *
         * This in turn toggles fullscreen display scaling
         *
         * @private
         */
        // _onFullscreenChange() {
        //     var fullscreenElement = document.webkitFullscreenElement || document.fullscreenElement || document.mozFullScreenElement || document.msFullscreenElement;

        //     this.isFullscreen = fullscreenElement === this.target;

        //     if (!this.isFullscreen) {
        //         // Dom(this.target).css({
        //         //     'transform': 'scale(1.0, 1.0)'
        //         // });
        //         Dom(this.target).css({
        //             width: `${this.width}px`,
        //             height: `${this.height}px`
        //         }).find('canvas').css({
        //             width: `${this.width}px`,
        //             height: `${this.height}px`,
        //             top: 0,
        //             left: 0
        //         });
        //     } else {
        //         const size = this._getFullScreenSize(this.width, this.height);
        //         console.log('size', size.scaleX, size.scaleY);
        //         Dom(this.target).css({
        //             width: `${size.width}px`,
        //             height: `${size.height}px`
        //         }).find('canvas').css({
        //             width: size.width + 'px',
        //             height: size.height + 'px',
        //             top: size.top + 'px',
        //             left: size.left + 'px'
        //         });
        //         // Dom(this.target).css({
        //         //     'transform': `scale(${size.scaleX}, ${size.scaleY})`
        //         // });

        //     }
        // }

        /**
         * Computes the fullscreen size: depending on the browser/device,
         * there are different ways to get correct fullscreen pixel size
         *
         * @param {Number} width initial width of the screen
         * @param {Number} height initial height of the screen
         *
         * @returns {Object} with new width, height, and x/y scale ratios
         */
        // _getFullScreenSize(width, height) {
        //     var ratio = width / height,
        //         needMargin = navigator.userAgent.match(/Edge|Firefox/),
        //         isXbox = navigator.userAgent.match(/Edge/),
        //         screenWidth = screen.width,
        //         screenHeight = screen.height,
        //         newWidth,
        //         newHeight;
        //     // both Firefox & Edge force fullscreen element to full screen size
        //     // since our canvas element do not necessarilty take the whole screen
        //     // we have to center them


        //     if (isXbox) {
        //         screenWidth = window.innerWidth || document.documentElement.clientWidth || document.body.clientWidth;
        //         screenHeight = window.innerHeight || document.documentElement.clientHeight || document.body.clientHeight;
        //     }

        //     console.log('screen', screen.width, screen.height);

        //     // use height as base since it's
        //     if (ratio > 0) {
        //         newHeight = screenHeight;
        //         newWidth = newHeight * ratio;
        //     } else {
        //         newWidth = screenWidth;
        //         newHeight = newWidth * ratio;
        //     }

        //     return {
        //         width: newWidth,
        //         height: newHeight,
        //         scaleX: newWidth / width,
        //         scaleY: newHeight / height,
        //         top: needMargin ? (screenHeight - newHeight) / 2 : 0,
        //         left: needMargin ? (screenWidth - newWidth) / 2 : 0
        //     };
        // }

        /**
         * Toggles fullscreen display scaling
         */
        toggleFullscreen(): void;

        /**
         * Create game layers.
         *
         * This method will create this.layers.length layers plus one more
         * used for post-rendering effects
         *
         * @private
         */
        // _createLayers() {
        //     let i;

        //     for (i = 0; i < this.layers.length; ++i) {
        //         this.layers[i] = Dom('canvas').addClass('layer_' + i).attr({
        //             'width': this.width,
        //             'height': this.height
        //         }).css({
        //             'width': this.width + 'px',
        //             'height': this.height + 'px',
        //             'position': 'absolute',
        //             zIndex: this._getLayerZIndex(i)
        //         }).appendTo(this.target)[0].getContext(this.type);

        //         this.layers[i]['imageSmoothingEnabled'] = false;
        //     }

        //     this.fxCtx = Dom('canvas').addClass('fx').attr({
        //         'width': this.width,
        //         'height': this.height
        //     }).css({
        //         'width': this.width + 'px',
        //         'height': this.height + 'px',
        //         'position': 'absolute',
        //         zIndex: 3
        //     }).appendTo(this.target)[0].getContext(this.type);

        //     this.fxCtx['imageSmoothingEnabled'] = false;
        // }

        /**
         * Returns the zIndex property of the specified layer canvas
         *
         * @param {Number} layer The layer number.
         *
         * @private
         */
        // _getLayerZIndex(layer) {
        //     // normal layer
        //     if (layer < this.layersIndex.length) {
        //         const isBackground = this.layersIndex[layer];
        //         return isBackground ? 0 : 2;
        //     } else {
        //         // map is always set to 1 for now
        //         return 1;
        //     }
        // }

        /**
         * Sorts the layers by zIndex + DOM position
         *
         * @note: We need to keep track of the rendering order of the layers
         * because the 'post' effects need the composited layer
         */
        // _sortLayers() {
        //     // first we need to render background layers
        //     let sortedLayers = [];

        //     this.layersIndex.forEach((isBackground, index) => {
        //         if (isBackground) {
        //             sortedLayers.push(index);
        //         }
        //     });
        //     // then map
        //     sortedLayers.push(this.layers.length - 1);

        //     // then forground layers
        //     this.layersIndex.forEach((isBackground, index) => {
        //         if (!isBackground) {
        //             sortedLayers.push(index);
        //         }
        //     });

        //     return sortedLayers;
        // }

        /**
         * Changes the zIndex property of the specified layer canvas
         *
         * @param {Number} layer The layer number.
         * @param {Number} zIndex The new zIndex value for this layer
         */
        setLayerZIndex(layer: number, zIndex: number): void;

        /**
         * Clears a canvas display buffer
         *
         * @param {RenderingContext} ctx The context to clear
         */
        clearScreen(ctx: RenderingContext): void;

        /**
         * Clears every rendering buffer, including the special fxCtx one
         */
        clearAllScreens(): void;

        /**
         * Changes the (CSS) opacity of a canvas
         *
         * @param {Canvas} canvas The Canvas HTML element.
         * @param {Number} opacity The new opacity value for this canvas.
         */
        setCanvasOpacity(canvas: HTMLElement, opacity: number): void;

        /**
         * Renders the specified scene
         *
         * @param {Scene} scene the scene to render
         */
        renderScene(scene: Scene): void;

        /**
         * Prepares the canvas before rendering images.
         *
         * @param {Array} resources Array of resources to use.
         *
         * Explanation: during development, I noticed that the very first time
         * the ctx.drawImage() was used to draw onto a canvas, it took a very long time,
         * like at least 10ms for a very small 32x32 pixels drawImage.
         *
         * Subsequent calls do not have this problem and are instant.
         * Maybe some ColorFormat conversion happens.
         *
         * This method makes sure that when the game starts rendering, we don't have
         * any of these delays that can impact gameplay and alter the gameplay experience
         * in a negative way.
         */
        prepareCanvas(resources: object[]): void;

        /**
         * Starts an animation on the display
         *
         * @param {String} fxName Name of the effect to apply.
         * @param {Object} options
         * @param {String} [options.easing='linear'] The easing method to use
         * @param {String} [options.when='pre'] When is the effect applied: can be before the game frame rendering ('pre') or after ('post')
         * @param {any} [options.context=this] The context (this) to apply to the animation.
         * @param {any} context The context to bind the Effect to
         */
        animate(fxName: string, options: object, context: RenderingContext): Promise;

        /**
         * stops current animation
         *
         * TODO
         * @private
         */
        stopAnimate(fxName?: string): void;

        /**
         * Executes an effect on a frame at a given time
         *
         * @param {RenderingContext} ctx Context that contains current frame rendering.
         * @param {RenderingContext} fxCtx The context in which to render the transformed frame.
         * @param {any} obj The object on which animation is applied: should be a `Drawable`.
         * @param {any} time Unused.
         * @param {String} when is this effect executed: 'pre' means before rendering, 'post' means after frame render.
         */
        executeFx(ctx: RenderingContext, fxCtx: RenderingContext, obj: Drawable, time: number, when: string): void;

        /**
         * Clears every display layer and clears fx queues
         */
        clearDisplay(): void;
    }

    export const InputManager:_InputManager;

    export class MapEvent {
        /**
         * Creates a new MapEvent
         *
         * @param {Map} map
         */
        constructor(map:Map);

        /**
         * Resets the MapEvent switches, events and items
         */
        reset():void;

        /**
         * Adds a new [`Drawable`]{#item} onto the map
         *
         * @param {String} id of the item to add
         * @param {Drawable} item to add
         */
        addItem(id:string, item:Drawable):void;

        /**
         * Returns an item
         *
         * @param {String} id of the item to retrieve
         *
         * @returns {Drawable|undefined} The item or undefined if it wasn't handled by the map
         */
        getItem(id:string):Drawable|undefined;

        // TODO: ability to trigger an event once a switch has been modified
        setSwitch(id:string, bool:boolean):void;

        toggleSwitch(id:string):void;

        /**
         * Retrieves a switch from the map using its id
         *
         * @param {String} id The switch to retrieve.
         *
         * @returns {any} returns the switch or false if it could not be found
         */
        getSwitch(id:string):any;

        /**
         * checks of conditions of specified trigger are valid
         *
         * @param {Object} trigger The trigger to check.
         *
         * @returns {boolean} true if the trigger is valid
         */
        checkConditions(trigger:object):boolean;

        handleAction(options:object):void;

        handleEvent(options:object):boolean;

        /**
         * Schedule adding a new object to the map
         *
         * @param {String} spriteId The id of the new sprite to add.
         * @param {Object} spriteOptions The options that will be passed to the object constructor.
         * @param {Number} [delay=0] The delay in milliseconds to wait before adding the object.
         * @returns {Drawable} the new drawable
         *
         */
        scheduleSprite(spriteId:string, spriteOptions:object, delay:number):Drawable;


        /**
         * Add a new wave of objects to the map
         * Used for example when the player triggers apparition of several enemies or bonuses
         *
         * @param {Object} options The options to pass to the wav object
         * @returns {boolean}
         *
         * @related {Wave}
         */
        handleWave(options:object):boolean;

        endWave():void;

        triggerEvent(id:string):void;

        isEventTriggered(id:string):boolean;
    }


    export class Behavior {
        vx:number;
        vy:number
        gravity:number;
        sprite:Drawable;
        constructor(sprite:Drawable, options:BehaviorOptions);
        onUpdate(timestamp:number):void;
        onVXChange?(vx:number):void;
        onVYChange?(vy:number):void;

        /**
         * Returns current mapEvent
         *
         * @returns {MapEvent} the object's current map event
         */
        getMapEvent(): MapEvent;
        reset():void;
    }

    interface _AudioManager {
        audioCache: object,
        enabled: boolean,
        /**
         * Adds a new sound element to the audio cache.
         * *Note* if a sound with the same id has already been added, it will be replaced
         * by the new one.
         *
         * @param {String} id
         * @param {HTMLAudioElement} element
         */
        addSound(id: string, element: HTMLAudioElement): void;
    /**
     * Toggles global sound playback
     *
     * @param {boolean} bool whether to enabled or disable sound playback.
     */
        toggleSound(bool: boolean): void;
    /**
     * Plays the specified sound with `id`.
     *
     * @param {String} id The id of the sound to play.
     * @param {boolean} [loop=false] Set to true to have the sound playback loop.
     * @param {Number} [volume=1] a Number between 0 and 1.
     * @param {Number} [panning=0] a Number between 10 (left) and -10 (right).
     * @returns {Wad} the created sound instance
     */
        play(id: string, loop?: boolean, volume?: number, panning?: number): any;
    /**
     * Stops playing the sound id
     *
     * @param {String} id The id of the sound to stop playing.
     * @param {any} instanceId The instanceId to use, in case several sounds with the same Id are being played.
     *
     * @returns {undefined}
     */
        stop(id: string, instanceId: any): void;
    }

    export const AudioManager: _AudioManager;

    interface Res {
        id: string,
        type: string,
        src: string
    }

    export type Callback = (...args: any[]) => void;

    interface _NotificationManager {
        notify(name: string, data?: JSObject): void;
    }

    export const NotificationManager: _NotificationManager;

    interface _ResourceManager {
        addResources(resource: Res, group?: string): Promise;
        getCanvasFromImage(image: HTMLImageElement): HTMLCanvasElement;
        getResourceById(id: string, group?: string, fullObject?: boolean): any;
        loadResources(group: string, progressCb?: Callback, errorCb?: Callback): void;
        loadImage(res: Res, group?: string): Promise;
        loadAudio(res: Res, group?: string): Promise;
        newResourceFromPool(id: string): any;
        registerScript(id: string, elt: any, poolSize?: number): void;
    }

    export const ResourceManager: _ResourceManager;

    interface _InputManager {
 /**
     * A list of common keyCodes
     */
    KEYS: {
        'UP': 38,
        'DOWN': 40,
        'LEFT': 37,
        'RIGHT': 39,
        'SPACE': 32,
        'ENTER': 13,
        'ESCAPE': 27,
        'CTRL': 17
    };
    /**
     * List of common pad buttons
     */
    PAD_BUTTONS: {
        32: 1, // Face (main) buttons
        FACE_0: 1,
        FACE_3: 2,
        FACE_4: 3,
        LEFT_SHOULDER: 4, // Top shoulder buttons
        RIGHT_SHOULDER: 5,
        LEFT_SHOULDER_BOTTOM: 6, // Bottom shoulder buttons
        RIGHT_SHOULDER_BOTTOM: 7,
        SELECT: 8,
        START: 9,
        LEFT_ANALOGUE_STICK: 10, // Analogue sticks (if depressible)
        RIGHT_ANALOGUE_STICK: 11,
        38: 12, // Directional (discrete) pad
        40: 13,
        37: 14,
        39: 15
    };
    axes: object;
    newGamepadPollDelay: number;
    gamepadSupport: boolean;
    recording: boolean;
    playingEvents: boolean;
    playingPos: number;
    /*recordedEvents: Array,*/
    pad: null;
    latches: object;
    keyPressed: object;
    padPressed: object;
    keyCb: object;
    enabled: boolean;
    inputMode:string;
    // virtual joystick instance
    dPadJoystick: null;
    jPollInterval: number;
    /**
     * Initializes the InputManager with a reference to the game.
     *
     * This method prepares the InputManager by reseting keyboard states/handlers and
     * set current inputMode
     *
     * @param {Object} options List of input options, unused for now
     *
     */
    init():void
    /**
     * generates key char from key codes
     *
     * @private
     */
    // _generateKeyCodes: function ():void {
    //     for (let i = 65; i < 91; ++i) {
    //         this.KEYS[String.fromCharCode(i)] = i;
    //     }
    // },
    /**
     * Private handler that is supposed to detect touchEvent and automatically switch between keyboard & touch
     * inputs. Unfortunately it tourned out to not be so easy.
     *
     * @private
     */
    // _installInputModeSwitchHandler: function () {
    //     // we cannot have several input devices (ie: keyboard, joystick,...) running at the same time
    //     // since they will interfer with each other (pressing a key will stop touch from working correctly)
    //     // we don't want the user to have to choose input mode using a menu or shortcut
    //     // instead, we want to have an automatic detection/switch of input mode which works like this:
    //     // by default, input mode if set to keyboard
    //     // if a touch is detected, input is set to joystick and kb detection is disabled
    //     // if a keydown is detected, joystick mode is disabled and kb detection is enabled
    //     document.addEventListener('touchstart', () => {
    //         this.setInputMode('joystick');
    //     });

    //     // document.addEventListener('keydown', () => {
    //     //     this.setInputMode('keyboard');
    //     // });
    // },
    /**
     * Starts recording input events. They are stored into `InputManager.recordedEvents`
     */
    startRecordingEvents():void;
    /**
     * Stops recording events.
     */
    stopRecordingEvents():void;
    /**
     * After events have been reccorded they can be played back using this method.
     */
    playRecordedEvents():void;
    /**
     * Sets next key states using recorded events
     *
     * TODO: add an optional callback to be called at the end of the playback
     * so that demo can be looped.
     */
    nextRecordedEvents():void;
    /**
     * Saves current event state onto the recordedEvents stack
     *
     * @private
     */
    // recordEvents: function () {
    //     /*            'UP': 38,
    //                 'DOWN': 40,
    //                 'LEFT': 37,
    //                 'RIGHT': 39,
    //                 'SPACE': 32,
    //                 'ENTER': 13,
    //                 'ESCAPE': 27,
    //                 'CTRL': 17*/
    //     this.recordedEvents.push(JSON.parse(JSON.stringify(this.keyPressed)));
    // },
    /**
     * Changes input mode
     *
     * @param {String} mode Changes current input mode, can be `virtual_joystick`, `keyboard`, `gamepad`
     */
    setInputMode(mode:string):void;
    /**
     * Resets keys that have been pressed.
     *
     * @private
     */
    // _resetKeys: function () {
    //     for (let key in this.keyPressed) {
    //         this.keyPressed[key] = false;
    //         this.latches[key] = false;
    //     }
    // },
    /**
     * Checks for a new joystick to be connected onto the machine and changes the inputMode to `gamepad`
     * when a new joypad is detected.
     *
     * @private
     */
    // _pollNewGamepad: function () {
    //     let gamepads = (navigator.getGamepads && navigator.getGamepads()) || (navigator.webkitGetGamepads && navigator.webkitGetGamepads()),
    //         pad = null;

    //     // TODO: we just use the first one for now, we need to be able to use any pad
    //     if (gamepads && gamepads.length) {
    //         for (let i = 0; i < gamepads.length; ++i) {
    //             pad = gamepads[i];
    //             if (pad) {
    //                 this.pad = pad;
    //                 if (!this.gamepadSupport) {
    //                     console.log('[Event] Oh oh! Looks like we have a new challenger: ', pad.id);
    //                     this.gamepadSupport = true;
    //                     this.setInputMode('gamepad');
    //                 }
    //             }
    //         }
    //     }

    //     if (!this.gamepadSupport) {
    //         setTimeout(() => {
    //             this._pollNewGamepad();
    //         }, this.newGamepadPollDelay);
    //     }
    // },
    /**
     * @private
     */
    // _pollGamepad: function () {
    //     // normal buttons
    //     // if (key === this.KEYS.space) {
    //     //     if (this.pad.buttons[this.PAD_BUTTONS[key]].pressed === true) {
    //     //         this.padPressed[key] = true;
    //     //     } else {
    //     //         this.padPressed[key] = false;
    //     //     }
    //     // }
    //     this._pollNewGamepad();

    //     // special case for dpad on Linux, cannot test on Windows since my pad does not support XInput...
    //     // d-pad
    //     // console.log('pressed', typeof this.pad.buttons[12].pressed, "**");
    //     // console.log('poll gamepad', typeof this.pad.buttons[12].pressed, this.pad.buttons[12].pressed.toString());
    //     // for (var i = 0; i < this.pad.buttons.length; ++i) {
    //     //     console.log(i, this.pad.buttons[i].pressed.toString());
    //     // }

    //     if (this.pad.buttons[12].pressed && !this.latches[this.KEYS['UP']]) {
    //         this.keyPressed[this.KEYS['UP']] = true;
    //         this.keyPressed[this.KEYS['DOWN']] = false;
    //     } else if (this.pad.buttons[13].pressed && !this.latches[this.KEYS['DOWN']]) {
    //         this.latches[this.KEYS['UP']] = false;
    //         this.keyPressed[this.KEYS['DOWN']] = true;
    //         this.keyPressed[this.KEYS['UP']] = false;
    //     } else {
    //         this.latches[this.KEYS['UP']] = false;
    //         this.latches[this.KEYS['DOWN']] = false;
    //         this.keyPressed[this.KEYS['DOWN']] = false;
    //         this.keyPressed[this.KEYS['UP']] = false;
    //     }

    //     if (this.pad.buttons[15].pressed && !this.latches[this.KEYS['RIGHT']]) {
    //         this.keyPressed[this.KEYS['RIGHT']] = true;
    //         this.keyPressed[this.KEYS['LEFT']] = false;
    //     } else if (this.pad.buttons[14].pressed) {
    //         this.latches[this.KEYS['RIGHT']] = false;
    //         this.keyPressed[this.KEYS['LEFT']] = true;
    //         this.keyPressed[this.KEYS['RIGHT']] = false;
    //     } else {
    //         this.latches[this.KEYS['RIGHT']] = false;
    //         this.latches[this.KEYS['LEFT']] = false;
    //         this.keyPressed[this.KEYS['LEFT']] = false;
    //         this.keyPressed[this.KEYS['RIGHT']] = false;
    //     }

    //     if (this.pad.buttons[0].pressed && !this.latches[this.KEYS['SPACE']]) {
    //         this.keyPressed[this.KEYS['SPACE']] = true;
    //     } else if (!this.pad.buttons[0].pressed) {
    //         this.latches[this.KEYS['SPACE']] = false;
    //         this.keyPressed[this.KEYS['SPACE']] = false;
    //     }

    //     if (this.pad.buttons[1].pressed && !this.latches[this.KEYS['CTRL']]) {
    //         this.keyPressed[this.KEYS['CTRL']] = true;
    //     } else if (!this.pad.buttons[1].pressed) {
    //         this.latches[this.KEYS['CTRL']] = false;
    //         this.keyPressed[this.KEYS['CTRL']] = false;
    //     }
    //     // stick 1
    //     /*
    //     if (this.pad.axes[1] === -1) {
    //         this.keyPressed[this.KEYS['UP']] = true;
    //         this.keyPressed[this.KEYS['DOWN']] = false;
    //     } else if (this.pad.axes[1] === 1) {
    //         this.keyPressed[this.KEYS['DOWN']] = true;
    //         this.keyPressed[this.KEYS['UP']] = false;
    //     } else {
    //         this.keyPressed[this.KEYS['DOWN']] = false;
    //         this.keyPressed[this.KEYS['UP']] = false;
    //     }
    //     if (this.pad.axes[0] === 1) {
    //         this.keyPressed[this.KEYS['RIGHT']] = true;
    //         this.keyPressed[this.KEYS['LEFT']] = false;
    //     } else if (this.pad.axes[0] === -1) {
    //         this.keyPressed[this.KEYS['LEFT']] = true;
    //         this.keyPressed[this.KEYS['RIGHT']] = false;
    //     } else {
    //         this.keyPressed[this.KEYS['LEFT']] = false;
    //         this.keyPressed[this.KEYS['RIGHT']] = false;
    //     }
    //     */
    //     this.jPollInterval = requestAnimationFrame(this._pollGamepad.bind(this));
    // },
    // _getModifiers: function (/*event*/) {
    //     return {
    //         'ALT': true,
    //         'SHIFT': true,
    //         'CTRL': true,
    //         'META': true
    //     };
    // },
    // _initVirtualJoystick: function () {
    //     let dPadJoystick,
    //         fireJoystick;

    //     console.log('[InputManager] _initVirtualJoystick');

    //     // left joystick = view
    //     dPadJoystick = this.dPadJoystick = new VirtualJoystick({
    //         container: document.body,
    //         strokeStyle: 'cyan',
    //         limitStickTravel: true,
    //         mouseSupport: true,
    //         stickRadius: 60
    //     });

    //     dPadJoystick.addEventListener('touchStartValidation', function (event) {
    //         let touch = event.changedTouches[0];
    //         if (touch.pageX >= window.innerWidth / 2) {
    //             return false;
    //         }
    //         return true;
    //     });

    //     // right joystick = fire button
    //     fireJoystick = this.fireJoystick = new VirtualJoystick({
    //         container: document.body,
    //         strokeStyle: 'orange',
    //         limitStickTravel: true,
    //         mouseSupport: true,
    //         stickRadius: 0
    //     });

    //     fireJoystick.addEventListener('touchStartValidation', function (event) {
    //         let touch = event.changedTouches[0];
    //         if (touch.pageX < window.innerWidth / 2) {
    //             return false;
    //         }
    //         return true;
    //     });

    //     /* fire button */
    //     fireJoystick.addEventListener('touchStart', () => {
    //         if (this.inputMode === 'virtual_joystick') {
    //             this.keyPressed[this.KEYS['CTRL']] = true;
    //         }
    //     });

    //     fireJoystick.addEventListener('touchEnd', () => {
    //         if (this.inputMode === 'virtual_joystick') {
    //             this.keyPressed[this.KEYS['CTRL']] = false;
    //         }
    //     });
    // },
    // _clearJoystickPoll: function () {
    //     if (this.jPollInterval) {
    //         // clearInterval(this.jPollInterval);
    //         cancelAnimationFrame(this.jPollInterval);
    //         this.jPollInterval = 0;
    //     }
    // },
    // _pollJoystick: function () {
    //     let down = [],
    //         up = [],
    //         joystick = this.dPadJoystick;

    //     /* directions */
    //     if (Math.abs(joystick.deltaX()) >= 10) {
    //         if (joystick.left()) {
    //             down.push('LEFT');
    //             up.push('RIGHT');
    //         } else {
    //             down.push('RIGHT');
    //             up.push('LEFT');
    //         }
    //     } else {
    //         up.push('LEFT');
    //         up.push('RIGHT');
    //     }

    //     if (Math.abs(joystick.deltaY()) >= 10) {
    //         if (joystick.up()) {
    //             down.push('UP');
    //             up.push('DOWN');
    //         } else {
    //             down.push('DOWN');
    //             up.push('UP');
    //         }
    //     } else {
    //         up.push('UP');
    //         up.push('DOWN');
    //     }

    //     if (down.length) {
    //         down.forEach((key) => {
    //             this.keyPressed[this.KEYS[key]] = true;
    //         });
    //     }

    //     if (up.length) {
    //         up.forEach((key) => {
    //             this.keyPressed[this.KEYS[key]] = false;
    //         });
    //     }

    //     // TODO: what happens for up event ? should be set to up only when going from down to up and called here
    // },
    /**
     * Intalls golbal keyboard events for `keydown` / `keyup` events
     * @private
     */
    // _installKBEventHandlers: function () {
    //     // TODO: move me somewhere else!
    //     document.addEventListener('keydown', (event) => {

    //         if (this.inputMode !== 'keyboard' || this.playingEvents) {
    //             return;
    //         }

    //         switch (event.keyCode) {
    //             case 32:
    //             case 37:
    //             case 38:
    //             case 39:
    //             case 40:
    //                 event.preventDefault();
    //                 break;
    //         }

    //         if (event.keyCode && !this.latches[event.keyCode]) {
    //             this.keyPressed[event.keyCode] = true;
    //         }

    //         // console.log('keydown', event.keyCode, '<-', this.keyPressed[37], '->', this.keyPressed[39]);

    //         this.metas = this._getModifiers();

    //         if (this.enabled && this.keyCb[event.keyCode]) {
    //             this.keyCb[event.keyCode].down.forEach((callback) => { callback(String.fromCharCode(event.keyCode), event); });
    //         }
    //     });

        //     document.addEventListener('keyup', (event) => {
        //         if (this.inputMode !== 'keyboard' || this.playingEvents) {
        //             return;
        //         }

        //         if (event.keyCode) {
        //             this.keyPressed[event.keyCode] = false;
        //             this.latches[event.keyCode] = false;
        //         }c

        //         // console.log('keyup', event.keyCode, '<-', this.keyPressed[37], '->', this.keyPressed[39]);

        //         this.metas = this._getModifiers();

        //         if (this.enabled && this.keyCb[event.keyCode]) {
        //             this.keyCb[event.keyCode].up.forEach((callback) => { callback(String.fromCharCode(event.keyCode), event); });
        //         }
        //     });
        // },
        /**
         * Returns an object with the state of all keys
         */
        getAllKeysStatus():object;
        getKeyStatus(key:string,  latch:boolean):boolean;
        isKeyDown(key:string|number, latch?:boolean):boolean;
        /**
         * Install callback that gets called when a key is pressed/released
         *
         * @param {String} key space-separated list of keys to listen for
         * @param {String} event to listen for: can be `up` or `down`
         * @param {Function} callback the function to call
         */
        installKeyCallback(key:string, event:string, callback:(key:string, event:string) => void):void;
        removeKeyCallback(key:string, event:string, callback:() => void):void;
        clearEvents():void;
    }

    export interface Promise {
        then(val:any): any;
    }

    /* Deferred */
    export class Deferred {
        constructor();
        /**
         * Creates and immediately resolves a new deferred.
         *
         * @param {any} val the value to resolve the promise with
         *
         *
         */
        static resolve(val: any): Promise;
        promise: Promise;
        reject(val: any): void;
        resolve(val: any): void;
    }

    /* Dom support */
    interface _Dom<TElement = HTMLElement> extends Iterable<TElement>{
        [key: number]: HTMLElement;
        length: number;
        css(prop: object): _Dom;
        css(prop: string): _Dom;
        css(prop: string, val: string): _Dom;
        find(selector: string): _Dom;
        appendTo(selector: string | _Dom): _Dom;
        attr(att: string | object, val?: string): _Dom;
        addClass(classes: string): _Dom;
        removeClass(classes: string): _Dom;
        html(str: string): _Dom;
        show(): _Dom;
        hide(): _Dom;
    }

    /* Game Support */
    export interface GameOptions{
        name: string,
        showFps: boolean,
        width: number,
        height: number,
        debug: boolean,
        scene?: Scene,
        target?: string | HTMLElement,
        sound?:boolean
    }

    interface SceneOptions{

    }

    interface DrawableOptions{
        objectId: number;
        layer: number;
    }

    interface SimpleTextOptions{

    }

    interface PaintOptions {

    }

    interface BitmapTextOptions{

    }

    interface SpriteOptions {
        x?: number,
        y?: number
        pool?: number,
        imageId?: string,
        canCollide?: boolean,
        canCollideFriendBullet?: boolean,
        animations?: JSObject,
        collideGroup?: number,
        map?: Map,
        data?: JSObject,
        objectId?:string
    }

    interface BehaviorOptions {

    }

    interface AnimOptions {
        numFrames: number,
        frameWidth: number,
        frameHeight: number,
        frameDuration: number,
        offsetX?: number,
        offsetY?: number
    }

    type JSObject = {
        [key: string]: any
    }

    export interface GameEvent {
        type: string,
        data: JSObject
    }
}