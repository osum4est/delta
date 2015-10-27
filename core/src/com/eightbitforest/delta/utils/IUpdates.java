package com.eightbitforest.delta.utils;

import com.badlogic.gdx.graphics.g2d.SpriteBatch;

/**
 * Created by osumf on 8/19/2015.
 */
public interface IUpdates {

    void update(float deltaTime);

    void render(SpriteBatch batch);

    void dispose();
}
