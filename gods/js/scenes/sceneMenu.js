import { Scene, Text, Sprite, Menu, AudioManager as AM, EventManager as EM } from 'athenajs';

		class sceneMenu extends Scene{
      constructor() {
          super({
              name: 'menu',
              resources: [
                  // images
                  { id: 'intro', type: 'image', src: 'gods/img/godsIntro.jpg' },

                  { id: 'restart', type: 'audio', src: 'gods/audio/restart.mp3' }
              ]
          });

          this.menuObject = new Menu('mainMenu', {
              title: 'Gods JS',
              color: 'white',
              menuItems: [
                  {
                      text: '> Start Game',
                      selectable: true,
                      visible: true,
        active: true
                  },
                  {
                      text: '> Cannot Select ;)',
                      selectable: true,
                      visible: true
                  }
              ]
          }).moveTo(350, 250);
			}
      
			onLoad() {
				super.onLoad();

        var that = this;

				console.log('[scene ' + this.name + '] ' + 'onLoad');

                // ** this.setBackgroundImage(Game.RM.getResourceById('intro', undefined, true).img.src);
			}
      start() {
        super.start();

        var that = this;

        EM.clearEvents();

        EM.installKeyCallback('DOWN', 'up', function() {
            console.log('down');
            that.menuObject.nextItem();
        });

        EM.installKeyCallback('ENTER', 'up', function() {
            console.log('selectedMenu', that.menuObject.getSelectedItemIndex());
            if (that.menuObject.getSelectedItemIndex() === 0) {
                that.notify('game:startGame');
            } else {
                that.notify('game:DTC!!!');
            }
        });

        AM.play('restart');

        this.addObject(this.menuObject);
      }
      stop() {
				EM.clearEvents();
        super.stop();
      }
      
			debug() {
			}
    };


    export default new sceneMenu();