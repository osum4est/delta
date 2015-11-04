package com.eightbitforest.delta.objects;

import com.badlogic.gdx.graphics.Color;
import com.badlogic.gdx.physics.box2d.Body;
import com.eightbitforest.delta.objects.base.GameObjectDynamic;
import com.eightbitforest.delta.objects.base.GameObjectDynamicTriangle;
import com.eightbitforest.delta.utils.ObjectType;
import com.eightbitforest.delta.utils.interfaces.ISpawnable;


public class ObjectAsteroid extends GameObjectDynamicTriangle implements ISpawnable {

    public ObjectAsteroid() {
        super(ObjectType.ASTEROID);
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
    public void update(float deltaTime) {

    }
}
