package com.eightbitforest.delta.utils.interfaces;

import com.badlogic.gdx.physics.box2d.Body;

/**
 * Created by osumf on 8/18/2015.
 */
public interface ISpawnable {

    float getSpawnChance();
    float getSpawnAmount();
    Body getBody();

    void dispose();
}
