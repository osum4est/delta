package com.eightbitforest.delta.objects;

import com.badlogic.gdx.graphics.Color;
import com.badlogic.gdx.math.Vector2;
import com.badlogic.gdx.physics.box2d.Body;
import com.eightbitforest.delta.objects.base.GameObjectDynamic;
import com.eightbitforest.delta.objects.base.GameObjectDynamicTriangle;
import com.eightbitforest.delta.utils.G;
import com.eightbitforest.delta.utils.interfaces.ISpawnable;
import com.eightbitforest.delta.utils.ObjectType;

public class ObjectEnemy extends GameObjectDynamicTriangle implements ISpawnable {


    @Override
    public int getId() {
        return ObjectType.ENEMY;
    }

    @Override
    public void update(float deltaTime) {
        body.setTransform(body.getPosition(), G.i.player.body.getPosition().sub(body.getPosition()).angleRad() + (float) Math.PI * 3 / 2);
        body.applyForceToCenter(new Vector2(0, 5).rotateRad(body.getAngle()), true);
    }

    @Override
    public Color getColor() {
        return Color.GREEN;
    }

    @Override
    public float getSpawnChance() {
        return .5f;
    }

    @Override
    public float getSpawnAmount() {
        return 10;
    }

    @Override
    public Body getBody() {
        return body;
    }

    @Override
    public void onCollide(GameObjectDynamic other) {
        if (other.id == ObjectType.PLAYER) {
            dispose();
        }
    }

}