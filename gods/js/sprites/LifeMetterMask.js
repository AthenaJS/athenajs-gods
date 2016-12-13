import { Sprite, ResourceManager as RM } from 'athenajs';

		class LifeMetterMask extends Sprite{
			constructor(options = {}) {
				super('lifemettermask', {
						imageSrc: 'objects',
						x: options.x,
						y: options.y,
						pool: options.pool,
						animations: {
							mainLoop: {
								frameDuration: 1,
								frames:[{
									offsetX: 400,
									offsetY: 239,
									w: 48,
									h: 24,
									plane: 0
								}],
                                loop: 0
							}
                        }
                });

                var that = this;

				// options = options || {};

				options.x = typeof options.x !== 'undefined' ? options.x : 600;
				options.y = typeof options.y !== 'undefined' ? options.y : 300;

                this.height = typeof options.height !== 'undefined' ? options.height : 0;

                this.running = true;
            }
            reset() {
				super.reset();

                this.currentMovement = '';
            }
            // override sprite's getCurrentHeight to alter height depending
            // on energy
            getCurrentHeight() {
                return this.height;
            }
		}

		RM.loadScript2('LifeMetterMask', LifeMetterMask);

		export default LifeMetterMask;