import { Map } from 'AthenaJS';
    // here we only extend Map to override addObject and keep track of switch objects
    class GodsMap extends Map{
        constructor(options) {
            super(options);

            this.reset();
        }
        
        reset() {
            this._switches = {};

            super.reset();
        }
        
        addObject(obj) {
            super.addObject(obj);

            // console.log('addObject', obj.type);

            // add switch to the list
            if (obj.type === 'switch') {
                this._switches[obj.id] = obj;
            }
        }
        
        getSwitchAboveMaster() {
            var switchSprite = null,
                master = this.masterObject,
                masterHitBox = master.getHitBox(),
                hitBox = null,
                masterCenterPos = master.x + (master.getCurrentWidth()/2),
                box = {
                    x: master.x,
                    y: master.y,
                    x2: master.x + masterHitBox.x2,
                    y2: master.y + masterHitBox.y2
                },
                id;

            for (id in this._switches) {
                switchSprite = this._switches[id];
                hitBox = switchSprite.getHitBox();

                if ((switchSprite.x + hitBox.x2) >= masterCenterPos && switchSprite.x <= masterCenterPos) {
                    if ((switchSprite.y  + hitBox.y2) >= (master.y - 5) && switchSprite.y <= (box.y2 + 5)) {
                        break;
                    }
                } else {
                    switchSprite = null;
                }
            }
            return switchSprite;
        }
    };

    export default GodsMap;

