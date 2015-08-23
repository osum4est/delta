package com.eightbitforest.delta.objects;

import com.badlogic.gdx.graphics.Color;
import com.badlogic.gdx.physics.box2d.Body;
import com.eightbitforest.delta.utils.ISpawnable;

/**
 * Created by osumf on 8/18/2015.
 */
public class ObjectAsteroid extends GameObjectDynamicTriangle implements ISpawnable {

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
}
