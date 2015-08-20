package com.eightbitforest.delta.objects;

import com.badlogic.gdx.graphics.Color;
import com.eightbitforest.delta.utils.ISpawnable;

/**
 * Created by osumf on 8/18/2015.
 */
public class ObjectAsteroid extends GameObjectDynamicTriangle implements ISpawnable {
    public ObjectAsteroid(Color color) {
        super(color);
    }

    @Override
    public float getSpawnChance() {
        return 1;
    }

    @Override
    public float getSpawnAmount() {
        return 1;
    }
}
