/*jshint esversion: 6*/
import { Sprite, AudioManager as AM, ResourceManager as RM } from 'athenajs';

		class Enemy extends Sprite{
			constructor(options) {
				super('enemy1', Object.assign({
                    x: 600,
                    y: 159,
                    canCollide: false,
					collideGroup: 1,
					canCollideFriendBullet: true,
                    imageSrc: 'enemies',
                    data: {
                        direction: 'Left',
                        health: 1,
                        speed: 1,
                    },
                    animations: {
                        mainLoopRight: {
                            frameDuration: 5,
                            frames:[{
                                offsetX: 594,
                                offsetY: 562,
                                w: 64,
                                h: 64,
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
                                w: 64,
                                h: 64,
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
                                w: 64,
                                h: 64,
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
                                w: 64,
                                h: 64,
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
                                w: 64,
                                h: 64,
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
                                w: 64,
                                h: 64,
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
                                w: 64,
                                h: 64,
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
                                w: 64,
                                h: 64,
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
                            frames:[{
                                offsetX: 1122,
                                offsetY: 628,
                                w: 64,
                                h: 64,
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
                                w: 64,
                                h: 64,
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
                                w: 64,
                                h: 64,
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
                                w: 64,
                                h: 64,
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
                                w: 64,
                                h: 64,
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
                                w: 64,
                                h: 64,
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
                                w: 64,
                                h: 64,
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
                                w: 64,
                                h: 64,
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
                            frames:[{
                                offsetX: 594,
                                offsetY: 694,
                                w: 64,
                                h: 64,
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
                                w: 64,
                                h: 64,
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
                                w: 64,
                                h: 64,
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
                                w: 64,
                                h: 64,
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
                                w: 64,
                                h: 64,
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
                                w: 64,
                                h: 64,
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
                                w: 64,
                                h: 64,
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
                                w: 64,
                                h: 64,
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
                            frames:[{
                                offsetX: 594,
                                offsetY: 364,
                                w: 64,
                                h: 64,
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
                                w: 64,
                                h: 64,
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
                                w: 64,
                                h: 64,
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
                                w: 64,
                                h: 64,
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
                                w: 64,
                                h: 64,
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
                                w: 64,
                                h: 64,
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
                                w: 64,
                                h: 64,
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
                                w: 64,
                                h: 64,
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
            reset(dir) {
				super.reset();

				AM.play('appearLeft');

				this.running = true;
				this.moving = false;

				this._visible = true;

				this.health = this._settings.data.health;
				this.speed = this._settings.data.speed;

                this.currentMovement = '';

				this.setAnimation('apparition', this.onAppeared.bind(this));
            }

			// called once enemy has finished apparitionAnimation
			onAppeared() {
				this.appeared = true;

				this.moving = true;

				this.canCollide = true;

				this.setAnimation(this._settings.data.direction === 'Left' ? 'mainLoopLeft' : 'mainLoopRight');

				this.setBehavior('ground', {
					vx: this._settings.data.direction === 'Left' ? -this.speed : this.speed,
					vy: 0,
					gravity: 0,
					onVXChange: (vx) => {
						if (vx < 0) {
							this.setAnimation('mainLoopLeft');
						} else {
							this.setAnimation('mainLoopRight');
						}
					}
				});
			}
			onCollision(sprite) {
				// TODO: add an Enemy class and inherit from this class so we do not have
				// to put code for each and every enemy variant
				if (this.canCollide) {
					this.moving = false;
					AM.play('explode1');
					this.canCollide = false;

					// group stuff
					if (this.wave) {
						this.wave.remove(this);
					}

					this.setAnimation('explosion', function() {
						this.destroy();
					});
				}
			}
		}

        RM.loadScript2('Enemy1', Enemy);

		export default Enemy;