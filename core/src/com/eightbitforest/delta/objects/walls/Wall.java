package com.eightbitforest.delta.objects.walls;

import com.badlogic.gdx.physics.box2d.BodyDef;
import com.eightbitforest.delta.level.Level;
import com.eightbitforest.delta.objects.base.BodyBuilder;
import com.eightbitforest.delta.objects.base.GameObjectPolygon;
import com.eightbitforest.delta.objects.base.ShapeBuilder;
import com.eightbitforest.delta.utils.Colors;
import com.eightbitforest.delta.utils.Ids;

public abstract class Wall extends GameObjectPolygon {
    public Wall(Level level, float x, float y) {
        super(level, Ids.WALL, x, y, Colors.WALL, false);

        BodyBuilder body = new BodyBuilder();
        body.setBodyType(BodyDef.BodyType.KinematicBody);
        ShapeBuilder shape = new ShapeBuilder();
        setShape(shape);
        body.setShape(shape);
        setBody(body);
    }

    protected abstract void setShape(ShapeBuilder shape);
}
