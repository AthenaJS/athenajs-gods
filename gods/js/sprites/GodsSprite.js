/*jshint esversion: 6*/
import { Sprite, NotificationManager as NM, AudioManager as AM, ResourceManager as RM } from 'athenajs';

// TODO: extend Sprite to create our special gods sprites here
class godsSprite extends Sprite {
    constructor(options) {
        super('gods', Object.assign(true, {
            imageId: 'sprites',
            pool: options.pool,
            canCollide: true,
            // inventory stuff
            data: {
                maxEnergy: 10,
                money: 0,
                carrying: [],
                weapon: ''
            },
            behavior: 'player',
            behaviorOptions: {
                animations: {
                    climbUp: 'climb'
                }
            },
            // animate: {
            // 	name: 'Rotate',
            // 	options: {
            // 		duration: 1000,
            // 		startValue: 0,
            // 		endValue: 2*Math.PI,
            // 		easing: 'linear',
            // 		loop: true
            // 	}
            // },
            animations: {
                standStill: {
                    frameDuration: 1,
                    frames: [{
                        offsetX: 396,
                        offsetY: 2,
                        width: 64,
                        height: 96,
                        hitBox: {
                            x: 1,
                            y: 4,
                            x2: 62,
                            y2: 95
                        },
                        plane: 0
                    }],
                    loop: 0
                },
                turnLeft: {
                    frameDuration: 4,
                    frames: [{
                        offsetX: 396,
                        offsetY: 2,
                        width: 64,
                        height: 96,
                        hitBox: {
                            x: 1,
                            y: 4,
                            x2: 62,
                            y2: 95
                        },
                        plane: 0
                    }, {
                        offsetX: 988,
                        offsetY: 298,
                        width: 64,
                        height: 96,
                        hitBox: {
                            x: 1,
                            y: 1,
                            x2: 62,
                            y2: 95
                        },
                        plane: 0
                    }],
                    loop: 0
                },
                turnRight: {
                    frameDuration: 4,
                    frames: [{
                        offsetX: 396,
                        offsetY: 2,
                        width: 64,
                        height: 96,
                        hitBox: {
                            x: 1,
                            y: 4,
                            x2: 62,
                            y2: 95
                        },
                        plane: 0
                    }, {
                        offsetX: 464,
                        offsetY: 2,
                        width: 64,
                        height: 96,
                        hitBox: {
                            x: 3,
                            y: 1,
                            x2: 60,
                            y2: 95
                        },
                        plane: 0
                    }],
                    loop: 0
                },
                faceWall: {
                    frameDuration: 1,
                    frames: [{
                        offsetX: 199,
                        offsetY: 99,
                        width: 64,
                        height: 96,
                        hitBox: {
                            x: 1,
                            y: 4,
                            x2: 62,
                            y2: 95
                        },
                        plane: 0
                    }],
                    loop: 0
                },
                jumpright: {
                    frameDuration: 1,
                    frames: [{
                        offsetX: 530,
                        offsetY: 98,
                        width: 64,
                        height: 96,
                        hitBox: {
                            x: 1,
                            y: 4,
                            x2: 62,
                            y2: 95
                        },
                        plane: 0
                    }, {
                        offsetX: 726,
                        offsetY: 98,
                        width: 64,
                        height: 96,
                        hitBox: {
                            x: 1,
                            y: 4,
                            x2: 62,
                            y2: 95
                        },
                        plane: 0
                    }, {
                        offsetX: 792,
                        offsetY: 98,
                        width: 64,
                        height: 96,
                        hitBox: {
                            x: 1,
                            y: 4,
                            x2: 62,
                            y2: 95
                        },
                        plane: 0
                    }],
                    loop: 0
                },
                jumpleft: {
                    frameDuration: 1,
                    frames: [{
                        offsetX: 302,
                        offsetY: 298,
                        width: 64,
                        height: 96,
                        hitBox: {
                            x: 1,
                            y: 4,
                            x2: 60,
                            y2: 95
                        },
                        plane: 0
                    }, {
                        offsetX: 230,
                        offsetY: 298,
                        width: 64,
                        height: 96,
                        hitBox: {
                            x: 1,
                            y: 4,
                            x2: 60,
                            y2: 95
                        },
                        plane: 0
                    }, {
                        offsetX: 166,
                        offsetY: 298,
                        width: 64,
                        height: 96,
                        hitBox: {
                            x: 1,
                            y: 4,
                            x2: 60,
                            y2: 95
                        },
                        plane: 0
                    }],
                    /*                            flipFrom: 'jumpright',
                                                flipType: 1,*/
                    loop: 0
                },
                climb: {
                    frameDuration: 5,
                    frames: [{
                        offsetX: 200,
                        offsetY: 98,
                        width: 64,
                        height: 96,
                        hitBox: {
                            x: 2,
                            y: 4,
                            x2: 62,
                            y2: 95
                        },
                        plane: 0
                    }, {
                        offsetX: 264,
                        offsetY: 98,
                        width: 64,
                        height: 96,
                        hitBox: {
                            x: 2,
                            y: 4,
                            x2: 62,
                            y2: 95
                        },
                        plane: 0
                    }, {
                        offsetX: 328,
                        offsetY: 98,
                        width: 64,
                        height: 96,
                        hitBox: {
                            x: 2,
                            y: 4,
                            x2: 62,
                            y2: 95
                        },
                        plane: 0
                    }, {
                        offsetX: 392,
                        offsetY: 98,
                        width: 64,
                        height: 96,
                        hitBox: {
                            x: 2,
                            y: 4,
                            x2: 62,
                            y2: 95
                        },
                        plane: 0
                    }],
                    loop: 1,
                    loopFrom: 1
                },
                fireright: {
                    frameDuration: 2,
                    frames: [{
                        offsetX: 0,
                        offsetY: 98,
                        width: 64,
                        height: 96,
                        hitBox: {
                            x: 1,
                            y: 4,
                            x2: 62,
                            y2: 95
                        },
                        plane: 0
                    }, {
                        offsetX: 66,
                        offsetY: 98,
                        width: 64,
                        height: 96,
                        hitBox: {
                            x: 1,
                            y: 4,
                            x2: 62,
                            y2: 95
                        },
                        plane: 0
                    }, {
                        offsetX: 132,
                        offsetY: 98,
                        width: 64,
                        height: 96,
                        hitBox: {
                            x: 1,
                            y: 4,
                            x2: 62,
                            y2: 95
                        },
                        plane: 0
                    }],
                    rewindOnEnd: true,
                    loop: 0
                },
                fireleft: {
                    frameDuration: 2,
                    frames: [{
                        offsetX: 132,
                        offsetY: 0,
                        width: 64,
                        height: 96,
                        hitBox: {
                            x: 1,
                            y: 4,
                            x2: 62,
                            y2: 95
                        },
                        plane: 0
                    }, {
                        offsetX: 66,
                        offsetY: 0,
                        width: 64,
                        height: 96,
                        hitBox: {
                            x: 1,
                            y: 4,
                            x2: 62,
                            y2: 95
                        },
                        plane: 0
                    }, {
                        offsetX: 0,
                        offsetY: 0,
                        width: 64,
                        height: 96,
                        hitBox: {
                            x: 1,
                            y: 4,
                            x2: 62,
                            y2: 95
                        },
                        plane: 0
                    }],
                    rewindOnEnd: true,
                    loop: 0,
                },
                fallleft: {
                    frameDuration: 1,
                    frames: [{
                        offsetX: 100,
                        offsetY: 298,
                        width: 64,
                        height: 96,
                        hitBox: {
                            x: 3,
                            y: 1,
                            x2: 60,
                            y2: 95
                        },
                        plane: 0
                    }],
                    loop: 0
                },
                fallright: {
                    frameDuration: 1,
                    frames: [{
                        offsetX: 858,
                        offsetY: 100,
                        width: 64,
                        height: 96,
                        hitBox: {
                            x: 3,
                            y: 1,
                            x2: 60,
                            y2: 95
                        },
                        plane: 0
                    }],
                    loop: 0
                },
                goDownright: {
                    frameDuration: 1,
                    frames: [{
                        offsetX: 464,
                        offsetY: 98,
                        width: 64,
                        height: 96,
                        hitBox: {
                            x: 2,
                            y: 20,
                            x2: 58,
                            y2: 95
                        },
                        plane: 0
                    }, {
                        offsetX: 530,
                        offsetY: 98,
                        width: 64,
                        height: 96,
                        hitBox: {
                            x: 2,
                            y: 20,
                            x2: 58,
                            y2: 95
                        },
                        plane: 0
                    }],
                    loop: 0,
                    rewindOnEnd: true
                },
                goDownleft: {
                    frameDuration: 1,
                    frames: [{
                        offsetX: 384,
                        offsetY: 298,
                        width: 64,
                        height: 96,
                        hitBox: {
                            x: 10,
                            y: 20,
                            x2: 60,
                            y2: 95
                        },
                        plane: 0
                    }, {
                        offsetX: 318,
                        offsetY: 298,
                        width: 64,
                        height: 96,
                        hitBox: {
                            x: 10,
                            y: 20,
                            x2: 60,
                            y2: 95
                        },
                        plane: 0
                    }],
                    loop: 0,
                    rewindOnEnd: true
                },
                walkRight: {
                    frameDuration: 3,
                    frames: [{
                        offsetX: 528,
                        offsetY: 2,
                        width: 64,
                        height: 96,
                        hitBox: {
                            x: 3,
                            y: 1,
                            x2: 60,
                            y2: 95
                        },
                        plane: 0
                    }, {
                        offsetX: 594,
                        offsetY: 2,
                        width: 64,
                        height: 96,
                        hitBox: {
                            x: 3,
                            y: 1,
                            x2: 60,
                            y2: 95
                        },
                        plane: 0
                    }, {
                        offsetX: 660,
                        offsetY: 2,
                        width: 64,
                        height: 96,
                        hitBox: {
                            x: 3,
                            y: 1,
                            x2: 60,
                            y2: 95
                        },
                        plane: 0
                    }, {
                        offsetX: 726,
                        offsetY: 2,
                        width: 64,
                        height: 96,
                        hitBox: {
                            x: 3,
                            y: 1,
                            x2: 60,
                            y2: 95
                        },
                        plane: 0
                    }, {
                        offsetX: 792,
                        offsetY: 2,
                        width: 64,
                        height: 96,
                        hitBox: {
                            x: 3,
                            y: 1,
                            x2: 60,
                            y2: 95
                        },
                        plane: 0
                    }, {
                        offsetX: 858,
                        offsetY: 2,
                        width: 64,
                        height: 96,
                        hitBox: {
                            x: 3,
                            y: 1,
                            x2: 60,
                            y2: 95
                        },
                        plane: 0
                    }, {
                        offsetX: 924,
                        offsetY: 2,
                        width: 64,
                        height: 96,
                        hitBox: {
                            x: 3,
                            y: 1,
                            x2: 60,
                            y2: 95
                        },
                        plane: 0
                    }, {
                        offsetX: 990,
                        offsetY: 2,
                        width: 64,
                        height: 96,
                        hitBox: {
                            x: 3,
                            y: 1,
                            x2: 60,
                            y2: 95
                        },
                        plane: 0
                    }],
                    rewindOnEnd: true, // should we rewind when reaching the last frame ?,
                    loopFrom: 0,
                    loop: 1
                },
                walkLeft: {
                    frameDuration: 3,
                    frames: [{
                        offsetX: 924,
                        offsetY: 298,
                        width: 64,
                        height: 96,
                        hitBox: {
                            x: 1,
                            y: 1,
                            x2: 62,
                            y2: 95
                        },
                        plane: 0
                    }, {
                        offsetX: 858,
                        offsetY: 298,
                        width: 64,
                        height: 96,
                        hitBox: {
                            x: 1,
                            y: 1,
                            x2: 62,
                            y2: 95
                        },
                        plane: 0
                    }, {
                        offsetX: 792,
                        offsetY: 298,
                        width: 64,
                        height: 96,
                        hitBox: {
                            x: 1,
                            y: 1,
                            x2: 62,
                            y2: 95
                        },
                        plane: 0
                    }, {
                        offsetX: 726,
                        offsetY: 298,
                        width: 64,
                        height: 96,
                        hitBox: {
                            x: 1,
                            y: 1,
                            x2: 62,
                            y2: 95
                        },
                        plane: 0
                    }, {
                        offsetX: 660,
                        offsetY: 298,
                        width: 64,
                        height: 96,
                        hitBox: {
                            x: 1,
                            y: 1,
                            x2: 62,
                            y2: 95
                        },
                        plane: 0
                    }, {
                        offsetX: 594,
                        offsetY: 298,
                        width: 64,
                        height: 96,
                        hitBox: {
                            x: 1,
                            y: 1,
                            x2: 62,
                            y2: 95
                        },
                        plane: 0
                    }, {
                        offsetX: 528,
                        offsetY: 298,
                        width: 64,
                        height: 96,
                        hitBox: {
                            x: 1,
                            y: 1,
                            x2: 62,
                            y2: 95
                        },
                        plane: 0
                    }, {
                        offsetX: 464,
                        offsetY: 298,
                        width: 64,
                        height: 96,
                        hitBox: {
                            x: 1,
                            y: 1,
                            x2: 62,
                            y2: 95
                        },
                        plane: 0
                    }],
                    rewindOnEnd: true, // should we rewind when reaching the last frame ?,
                    loop: 1,
                    loopFrom: 0
                }
            },
            frameDuration: 10,
            visible: true
        }, options));
    }
    reset() {
        super.reset();

        this.lookDirection = '';
        this.currentMovement = '';

        this.energy = this._settings.data.maxEnergy;

        this.lookDirection = '';
        this.currentMovement = '';

        this.setAnimation('standStill');

        this.canCollide = true;
        this.visible = true;
    }
    setAnimation(name, fn, frameNum, revert) {
        // console.log('[GodsSprite] Setting animation to', name);
        super.setAnimation(name, fn, frameNum, revert);
    }
    explode() { }
    checkKeys() { }
    onDamage(hitPoints) {
        // TODO: calculate here and passe it to hit ?
        this.energy -= hitPoints;

        if (this.energy <= 0) {
            this.energy = 0;
            // TODO
            // 1: destroy
            // 2: play death animation
            // 3: onEnd, call send death
            AM.play('death');

            this.canCollide = false;
            this.clearMove();
            this.visible = false;
            // TODO: add 5 random death elements
            // and remove the above line

            // *** NM.notify('player:death'); from sceneHud !!
            var death = new (RM.getResourceById('DeathExplosion'))({
                x: this.x - 20,
                y: this.y
            });

            death.onAnimationEnd(function () {
                death.visible = false;
                setTimeout(function () {
                    NM.notify('player:death');
                }, 700);
            });

            this.currentMap.addObject(death);
        } else {
            AM.play('hit2');
            // TODO: sprite should not collide for a few ms ?
        }

        NM.notify('player:hit', {
            damage: hitPoints
        });
    }
    onCollision(sprite) {
        switch (sprite.type) {
            case 'help':
                // console.log('sending game:message');
                NM.notify('game:message', {
                    message: sprite._settings.data.message
                });

                sprite.destroy();

                AM.play('take_bonus');
                break;

            case 'enemy1':
            case 'flying-enemy1':
            case 'spearWood':
                // TODO: call sprite.destroy() method (to be implemented)
                this.onDamage(sprite._settings.data.damage);
                break;

            case 'knife':
                // TODO: change weapon
                // TODO: play sound ?
                console.log('[GodsSprite] Need to catch knife');
                this.weapon = 'weapon';
                AM.play('take_bonus');
                // this.weapon = 'knife';
                sprite.destroy();
                break;

            default:
                sprite.destroy();
                AM.play('take_bonus');
                // console.log('onCollision called on non-handled sprite type', sprite.type);
                break;
        }
        super.onCollision();
    }
    onEvent(eventType, data) {
        // TODO: do we hold a weapon ?
        var result = false;

        switch (eventType) {
            case 'fire':
                if (this.weapon) {
                    var weapon = new (RM.getResourceById('Weapon'))({
                        x: data.direction === 'left' ? (this.x - 15) : (this.x + this.getCurrentWidth() - 15),
                        y: this.y + 20,
                        data: {
                            direction: data.direction
                        }
                    });
                    this.currentMap.addObject(weapon);
                    AM.play('weapon_throw');

                    result = true;
                }
                break;
        }

        return result;
    }
}

RM.registerScript('GodsSprite', godsSprite);

export default godsSprite;