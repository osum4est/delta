package com.eightbitforest.delta.objects.base;

import com.badlogic.gdx.graphics.Color;
import com.badlogic.gdx.math.Vector2;
import com.eightbitforest.delta.utils.ObjectType;

/**
 * Created by fjon2248 on 11/2/2015.
 */
public class Bullet extends GameObjectDynamicTriangle {

    public Bullet(float size) {
        super(size);
    }

    @Override
    public Color getColor() {
        return Color.MAGENTA;
    }

    @Override
    public int getId() {
        return ObjectType.BULLET;
    }

    @Override
    public void update(float deltaTime) {
        body.applyForceToCenter(new Vector2(0, 1).rotateRad(body.getAngle()), true);
    }
}
