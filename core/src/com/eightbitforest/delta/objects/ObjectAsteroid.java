package com.eightbitforest.delta.objects;

import com.badlogic.gdx.graphics.Color;
import com.badlogic.gdx.physics.box2d.Body;
import com.eightbitforest.delta.utils.ISpawnable;
import com.eightbitforest.delta.utils.ObjectType;

/**
 * Created by osumf on 8/18/2015.
 */
public class ObjectAsteroid extends GameObjectDynamicTriangle implements ISpawnable {

    @Override
    int getId() {
        return ObjectType.ASTEROID;
    }

    @Override
    public Color getColor() {
        return Color.valueOf("FFFFFF");
    }

    @Override
    public float getSpawnChance() {
        return 0.5f;
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

    @Override
    public void update(float deltaTime) {

    }
}
