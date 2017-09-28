package com.eightbitforest.delta.objects;

import com.eightbitforest.delta.level.Level;
import com.eightbitforest.delta.objects.base.GameObjectPolygon;
import com.eightbitforest.delta.utils.Colors;
import com.eightbitforest.delta.utils.Constants;
import com.eightbitforest.delta.utils.Ids;

public class Enemy extends GameObjectPolygon {
    public Enemy(Level level, float x, float y) {
        super(level, Ids.ENEMY, x, y, Colors.GREEN, Constants.TRIANGLE_HEIGHT * .75f);
    }
}
