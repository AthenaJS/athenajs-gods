/*jshint esversion: 6*/
import { Sprite, AudioManager as AM, ResourceManager as RM, SpriteOptions, Drawable } from 'athenajs';

class Enemy extends Sprite {
    health: number;
    speed: number;
    // unused ??
    _visible: boolean;
    appeared: boolean;

    constructor(options = { x: 0, y: 0, pool: undefined }) {
        super('enemy1', Object.assign({
            x: 600,
            y: 159,
            canCollide: false,
            collideGroup: 1,
            canCollideFriendBullet: true,
            imageId: 'enemies',
            data: {
                direction: 'Left',
                health: 1,
                speed: 1,
            },
            animations: {
                mainLoopRight: {
                    frameDuration: 5,
                    frames: [{
                        offsetX: 594,
                        offsetY: 562,
                        width: 64,
                        height: 64,
                        hitBox: {
                            x: 6,
                            y: 0,
                            x2: 57,
                            y2: 63
                        },
                        plane: 0
                    },
                    {
                        offsetX: 660,
                        offsetY: 562,
                        width: 64,
                        height: 64,
                        hitBox: {
                            x: 6,
                            y: 0,
                            x2: 57,
                            y2: 63
                        },
                        plane: 0
                    },
                    {
                        offsetX: 726,
                        offsetY: 562,
                        width: 64,
                        height: 64,
                        hitBox: {
                            x: 6,
                            y: 0,
                            x2: 57,
                            y2: 63
                        },
                        plane: 0
                    },
                    {
                        offsetX: 792,
                        offsetY: 562,
                        width: 64,
                        height: 64,
                        hitBox: {
                            x: 6,
                            y: 0,
                            x2: 57,
                            y2: 63
                        },
                        plane: 0
                    },
                    {
                        offsetX: 858,
                        offsetY: 562,
                        width: 64,
                        height: 64,
                        hitBox: {
                            x: 6,
                            y: 0,
                            x2: 57,
                            y2: 63
                        },
                        plane: 0
                    },
                    {
                        offsetX: 924,
                        offsetY: 562,
                        width: 64,
                        height: 64,
                        hitBox: {
                            x: 6,
                            y: 0,
                            x2: 57,
                            y2: 63
                        },
                        plane: 0
                    },
                    {
                        offsetX: 990,
                        offsetY: 562,
                        width: 64,
                        height: 64,
                        hitBox: {
                            x: 6,
                            y: 0,
                            x2: 57,
                            y2: 63
                        },
                        plane: 0
                    },
                    {
                        offsetX: 1056,
                        offsetY: 562,
                        width: 64,
                        height: 64,
                        hitBox: {
                            x: 6,
                            y: 0,
                            x2: 57,
                            y2: 63
                        },
                        plane: 0
                    }
                    ],
                    loop: 1
                },
                mainLoopLeft: {
                    frameDuration: 5,
                    frames: [{
                        offsetX: 1122,
                        offsetY: 628,
                        width: 64,
                        height: 64,
                        hitBox: {
                            x: 6,
                            y: 0,
                            x2: 57,
                            y2: 63
                        },
                        plane: 0
                    },
                    {
                        offsetX: 1056,
                        offsetY: 628,
                        width: 64,
                        height: 64,
                        hitBox: {
                            x: 6,
                            y: 0,
                            x2: 57,
                            y2: 63
                        },
                        plane: 0
                    },
                    {
                        offsetX: 990,
                        offsetY: 628,
                        width: 64,
                        height: 64,
                        hitBox: {
                            x: 6,
                            y: 0,
                            x2: 57,
                            y2: 63
                        },
                        plane: 0
                    },
                    {
                        offsetX: 924,
                        offsetY: 628,
                        width: 64,
                        height: 64,
                        hitBox: {
                            x: 6,
                            y: 0,
                            x2: 57,
                            y2: 63
                        },
                        plane: 0
                    },
                    {
                        offsetX: 858,
                        offsetY: 628,
                        width: 64,
                        height: 64,
                        hitBox: {
                            x: 6,
                            y: 0,
                            x2: 57,
                            y2: 63
                        },
                        plane: 0
                    },
                    {
                        offsetX: 792,
                        offsetY: 628,
                        width: 64,
                        height: 64,
                        hitBox: {
                            x: 6,
                            y: 0,
                            x2: 57,
                            y2: 63
                        },
                        plane: 0
                    },
                    {
                        offsetX: 726,
                        offsetY: 628,
                        width: 64,
                        height: 64,
                        hitBox: {
                            x: 6,
                            y: 0,
                            x2: 57,
                            y2: 63
                        },
                        plane: 0
                    },
                    {
                        offsetX: 660,
                        offsetY: 628,
                        width: 64,
                        height: 64,
                        hitBox: {
                            x: 6,
                            y: 0,
                            x2: 57,
                            y2: 63
                        },
                        plane: 0
                    }],
                    loop: 1
                },
                apparition: {
                    frameDuration: 3,
                    frames: [{
                        offsetX: 594,
                        offsetY: 694,
                        width: 64,
                        height: 64,
                        hitBox: {
                            x: 0,
                            y: 0,
                            x2: 63,
                            y2: 63
                        },
                        plane: 0
                    },
                    {
                        offsetX: 660,
                        offsetY: 694,
                        width: 64,
                        height: 64,
                        hitBox: {
                            x: 0,
                            y: 0,
                            x2: 63,
                            y2: 63
                        },
                        plane: 0
                    },
                    {
                        offsetX: 726,
                        offsetY: 694,
                        width: 64,
                        height: 64,
                        hitBox: {
                            x: 0,
                            y: 0,
                            x2: 63,
                            y2: 63
                        },
                        plane: 0
                    },
                    {
                        offsetX: 792,
                        offsetY: 694,
                        width: 64,
                        height: 64,
                        hitBox: {
                            x: 0,
                            y: 0,
                            x2: 63,
                            y2: 63
                        },
                        plane: 0
                    },
                    {
                        offsetX: 858,
                        offsetY: 694,
                        width: 64,
                        height: 64,
                        hitBox: {
                            x: 0,
                            y: 0,
                            x2: 63,
                            y2: 63
                        },
                        plane: 0
                    },
                    {
                        offsetX: 924,
                        offsetY: 694,
                        width: 64,
                        height: 64,
                        hitBox: {
                            x: 0,
                            y: 0,
                            x2: 63,
                            y2: 63
                        },
                        plane: 0
                    },
                    {
                        offsetX: 990,
                        offsetY: 694,
                        width: 64,
                        height: 64,
                        hitBox: {
                            x: 0,
                            y: 0,
                            x2: 63,
                            y2: 63
                        },
                        plane: 0
                    },
                    {
                        offsetX: 1056,
                        offsetY: 694,
                        width: 64,
                        height: 64,
                        hitBox: {
                            x: 0,
                            y: 0,
                            x2: 63,
                            y2: 63
                        },
                        plane: 0
                    }],
                    loop: 0
                },
                explosion: {
                    frameDuration: 3,
                    frames: [{
                        offsetX: 594,
                        offsetY: 364,
                        width: 64,
                        height: 64,
                        hitBox: {
                            x: 0,
                            y: 0,
                            x2: 63,
                            y2: 63
                        },
                        plane: 0
                    },
                    {
                        offsetX: 660,
                        offsetY: 364,
                        width: 64,
                        height: 64,
                        hitBox: {
                            x: 0,
                            y: 0,
                            x2: 63,
                            y2: 63
                        },
                        plane: 0
                    },
                    {
                        offsetX: 726,
                        offsetY: 364,
                        width: 64,
                        height: 64,
                        hitBox: {
                            x: 0,
                            y: 0,
                            x2: 63,
                            y2: 63
                        },
                        plane: 0
                    },
                    {
                        offsetX: 792,
                        offsetY: 364,
                        width: 64,
                        height: 64,
                        hitBox: {
                            x: 0,
                            y: 0,
                            x2: 63,
                            y2: 63
                        },
                        plane: 0
                    },
                    {
                        offsetX: 858,
                        offsetY: 364,
                        width: 64,
                        height: 64,
                        hitBox: {
                            x: 0,
                            y: 0,
                            x2: 63,
                            y2: 63
                        },
                        plane: 0
                    },
                    {
                        offsetX: 924,
                        offsetY: 364,
                        width: 64,
                        height: 64,
                        hitBox: {
                            x: 0,
                            y: 0,
                            x2: 63,
                            y2: 63
                        },
                        plane: 0
                    },
                    {
                        offsetX: 990,
                        offsetY: 364,
                        width: 64,
                        height: 64,
                        hitBox: {
                            x: 0,
                            y: 0,
                            x2: 63,
                            y2: 63
                        },
                        plane: 0
                    },
                    {
                        offsetX: 1056,
                        offsetY: 364,
                        width: 64,
                        height: 64,
                        hitBox: {
                            x: 0,
                            y: 0,
                            x2: 63,
                            y2: 63
                        },
                        plane: 0
                    }],
                    loop: 0
                }
            }
        }, options));
    }
    reset() {
        super.reset();

        AM.play('appearLeft');

        this.running = true;
        this.movable = false;

        this._visible = true;

        this.health = this.getProperty('health');
        this.speed = this.getProperty('speed');

        this.currentMovement = '';

        this.setAnimation('apparition', this.onAppeared.bind(this));
    }

    // called once enemy has finished apparitionAnimation
    onAppeared() {
        const direction = this.getProperty('direction');

        this.appeared = true;

        this.movable = true;

        this.canCollide = true;

        this.setAnimation(direction === 'Left' ? 'mainLoopLeft' : 'mainLoopRight');

        this.setBehavior('ground', {
            vx: direction === 'Left' ? -this.speed : this.speed,
            vy: 0,
            gravity: 0,
            onVXChange: (vx:number) => {
                if (vx < 0) {
                    this.setAnimation('mainLoopLeft');
                } else {
                    this.setAnimation('mainLoopRight');
                }
            }
        });
    }
    onCollision(sprite:Drawable) {
        // TODO: add an Enemy class and inherit from this class so we do not have
        // to put code for each and every enemy variant
        if (this.canCollide) {
            this.movable = false;
            AM.play('explode1');
            this.canCollide = false;

            this.setAnimation('explosion', function () {
                this.destroy();
            });
        }
    }
}

// RM.registerScript('Enemy1', Enemy);

export default Enemy;