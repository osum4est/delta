package com.eightbitforest.delta.objects;

import com.badlogic.gdx.graphics.Color;
import com.badlogic.gdx.math.Vector2;
import com.eightbitforest.delta.objects.base.GameObjectDynamicTriangle;
import com.eightbitforest.delta.utils.G;
import com.eightbitforest.delta.utils.ObjectType;

/**
 * Created by fjon2248 on 11/2/2015.
 */
public class Bullet extends GameObjectDynamicTriangle {

    float angle = 0;

    public Bullet(float size, float angle) {
        super(ObjectType.BULLET, size, G.i.CATEGORY_PLAYER, G.i.MASK_PLAYER);
        this.angle = angle;
    }

    @Override
    public Color getColor() {
        return Color.WHITE;
    }

    @Override
    public void update(float deltaTime) {
        body.setTransform(body.getPosition(), angle);
        body.setLinearVelocity(new Vector2(0, 25).rotateRad(angle));
    }
}