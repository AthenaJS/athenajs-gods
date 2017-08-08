/*jshint esversion: 6*/
import { Sprite, ResourceManager as RM } from 'athenajs';

class LifeMetter extends Sprite {
    constructor(options = {}) {
        // options = options || {};

        super('lifemetter', {
            imageSrc: 'objects',
            x: options.x,
            y: options.y,
            pool: options.pool,
            animations: {
                mainLoop: {
                    frameDuration: 4,
                    frames: [{
                        offsetX: 136,
                        offsetY: 227,
                        w: 48,
                        h: 48,
                        plane: 0
                    },
                    {
                        offsetX: 202,
                        offsetY: 227,
                        w: 48,
                        h: 48,
                        plane: 0
                    },
                    {
                        offsetX: 268,
                        offsetY: 227,
                        w: 48,
                        h: 48,
                        plane: 0
                    },
                    {
                        offsetX: 334,
                        offsetY: 227,
                        w: 48,
                        h: 48,
                        plane: 0
                    }],
                    loop: 1
                }
            }
        });

        options.x = typeof options.x !== 'undefined' ? options.x : 600;
        options.y = typeof options.y !== 'undefined' ? options.y : 300;

        this.maxEnergy = options.maxEnergy || 10;

        this.addMask();
    }
    /**
     * Resets the sprite to its default (start) state: full energy => mask.height == 0
     */
    reset() {
        debugger;
        this.currentMovement = '';
        this.setAnimation('mainLoop');

        this.running = true;
    }
    addMask() {
        // var MetterMask = require('sprites/LifeMetterMask').default;
        var MetterMask = RM.getResourceById('LifeMetterMask');

        this.maskSprite = new MetterMask({
            x: this.x,
            y: this.y + 12
        });

        this.addChild(this.maskSprite);
    }
    resetEnergy() {
        this.maskSprite.height = 0;
    }

    updateMetterHeight(hitPoints) {
        // TODO: this should be animated (inside lifeMetterMask ?)
        var maskSprite = this.maskSprite,
            height = maskSprite.height;

        maskSprite.height += 2 * hitPoints;

        if (maskSprite.height > 24) {
            maskSprite.height = 24;
        } else if (maskSprite.height <= 0) {
            maskSprite.height = 0;
        }

        /*                maskSprite.animate('Custom', {
                            duration: 400,
                            startValue: height,
                            endValue: height + (2 * hitPoints),
                            easing: 'linear',
                            callback: function(value) {
                                maskSprite.height = parseInt(value);
                            }
                        });*/
    }
}

RM.registerScript('LifeMetter', LifeMetter);

export default LifeMetter;