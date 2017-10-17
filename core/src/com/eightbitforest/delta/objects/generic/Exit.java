package com.eightbitforest.delta.objects.generic;

import com.badlogic.gdx.physics.box2d.BodyDef;
import com.eightbitforest.delta.level.Level;
import com.eightbitforest.delta.objects.base.BodyBuilder;
import com.eightbitforest.delta.objects.base.GameObjectPolygon;
import com.eightbitforest.delta.utils.Colors;
import com.eightbitforest.delta.utils.Ids;

public class Exit extends GameObjectPolygon {
    public Exit(Level level, float x, float y) {
        super(level, Ids.EXIT, x, y, Colors.EXIT, false);
        setBody(new BodyBuilder().setBodyType(BodyDef.BodyType.StaticBody).setSensor(true));
    }
}
