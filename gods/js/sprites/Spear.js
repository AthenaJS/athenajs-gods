import { Sprite, ResourceManager as RM } from 'athenajs';

class Spear extends Sprite {
    constructor(options) {
        super('spear', {
            imageId: 'objects',
            x: options.x,	// 790,
            y: options.y,	//480,
            pool: options.pool,
            canCollide: false,
            animations: {
                mainLoop: {
                    frameDuration: 2,
                    frames: [{
                        offsetX: 0,
                        offsetY: 227,
                        w: 32,
                        h: 32,
                        hitBox: {
                            x: 0,
                            y: 0,
                            x2: 31,
                            y2: 31
                        },
                        plane: 0
                    },
                    {
                        offsetX: 34,
                        offsetY: 227,
                        w: 32,
                        h: 32,
                        hitBox: {
                            x: 0,
                            y: 0,
                            x2: 31,
                            y2: 31
                        },
                        plane: 0
                    },
                    {
                        offsetX: 68,
                        offsetY: 227,
                        w: 32,
                        h: 32,
                        hitBox: {
                            x: 0,
                            y: 0,
                            x2: 31,
                            y2: 31
                        },
                        plane: 0
                    },
                    {
                        offsetX: 102,
                        offsetY: 227,
                        w: 32,
                        h: 32,
                        hitBox: {
                            x: 0,
                            y: 0,
                            x2: 31,
                            y2: 31
                        },
                        plane: 0
                    }],
                    loop: 1
                }
            }
        });

        var that = this;

        options = options || {};

        this.soundRef = null;
    }
    draw(ctx, debug) {
        // console.log(this.woodSprite.x, this.x);
        // this.woodSprite.draw(ctx, debug);
        super.draw(ctx, debug);
    }
    reset() {
        super.reset();

        this.currentMovement = '';
        this.setAnimation('mainLoop');

        this.setBehavior('inout', {
            vx: 0,
            vy: 1,
            minX: 0,
            minY: 16,
            gravity: 0,
        });
    }
};

RM.registerScript('Spear', Spear);

export default Spear;