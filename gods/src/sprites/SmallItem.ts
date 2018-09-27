import { Sprite, AudioManager as AM, ResourceManager as RM, Drawable, JSObject } from 'athenajs';

class SmallItem extends Sprite {
    constructor(options = { x: 0, y: 0, pool: undefined, vy: 0, data:{itemType: '', fall:false}}) {
        // if (options.pool) {
        //     return;
        // }

        // var itemType = options.data.itemType;

        super(options.data && options.data.itemType || '', {
            imageId: 'objects',
            x: options.x,
            y: options.y,
            pool: options.pool,
            canCollide: true,
            collideGroup: 1,
            data: options.data || {},
            animations: {
                mainLoop: {
                    frameDuration: 1,
                    frames: [{
                        offsetX: options.data && items[options.data.itemType].x,
                        offsetY: options.data && items[options.data.itemType].y,
                        width: 31,
                        height: 31,
                        hitBox: {
                            x: 13,
                            y: 14,// 3,
                            x2: 18,
                            y2: 31// 31
                        },
                        plane: 0
                    }],
                    loop: 0
                },
                disappear: {
                    frameDuration: 1,
                    frames: [{
                        offsetX: 272,
                        offsetY: 114,
                        width: 31,
                        height: 31,
                        hitBox: {
                            x: 0,
                            y: 0,
                            x2: 31,
                            y2: 31
                        },
                        plane: 0
                    },
                    {
                        offsetX: 306,
                        offsetY: 114,
                        width: 31,
                        height: 31,
                        hitBox: {
                            x: 0,
                            y: 0,
                            x2: 31,
                            y2: 31
                        },
                        plane: 0
                    },
                    {
                        offsetX: 340,
                        offsetY: 114,
                        width: 31,
                        height: 31,
                        hitBox: {
                            x: 0,
                            y: 0,
                            x2: 31,
                            y2: 31
                        },
                        plane: 0
                    },
                    {
                        offsetX: 374,
                        offsetY: 114,
                        width: 31,
                        height: 31,
                        hitBox: {
                            x: 0,
                            y: 0,
                            x2: 31,
                            y2: 31
                        },
                        plane: 0
                    },
                    {
                        offsetX: 408,
                        offsetY: 114,
                        width: 31,
                        height: 31,
                        hitBox: {
                            x: 0,
                            y: 0,
                            x2: 31,
                            y2: 31
                        },
                        plane: 0
                    },
                    {
                        offsetX: 442,
                        offsetY: 114,
                        width: 31,
                        height: 31,
                        hitBox: {
                            x: 0,
                            y: 0,
                            x2: 31,
                            y2: 31
                        },
                        plane: 0
                    },
                    {
                        offsetX: 476,
                        offsetY: 114,
                        width: 31,
                        height: 31,
                        hitBox: {
                            x: 0,
                            y: 0,
                            x2: 31,
                            y2: 31
                        },
                        plane: 0
                    }],
                    loop: 0
                }
            }
        });

        if (options.pool) {
            return;
        }

        if (options.data.fall || typeof options.data.fall === 'undefined') {
            this.setBehavior('simplefall', {
                gravity: .3,
                vy: options.vy || 0,
                onEnd: () => {
                    this.movable = false;
                },
                onGround: function () {
                    AM.play('bounce');
                }
            });
        }

        this.running = true;
    }
    destroy() {
        // since _super is only defined during destroy's lifetime, we need to save a reference to call later
        // var _sup = super.destroy.bind(this);

        // only call parent's super once disappear animation has been played
        this.setAnimation('disappear', () => {
            super.destroy();
        });
    }
    onCollision(sprite:Drawable) {
        this.canCollide = false;
    }
    reset() {
        super.reset();

        this.currentMovement = '';
        this.setAnimation('mainLoop');
    }
};

const items:JSObject = {
    'knife': {
        x: 456,
        y: 37
    },
    'life': {
        x: 238,
        y: 0
    },
    'help': {
        x: 0,
        y: 75
    },
    'apple': {
        x: 102,
        y: 75
    }
};

// RM.registerScript('SmallItem', SmallItem);

export default SmallItem;
