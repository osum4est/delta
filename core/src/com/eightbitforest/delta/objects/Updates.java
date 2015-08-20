package com.eightbitforest.delta.objects;

import com.badlogic.gdx.graphics.g2d.SpriteBatch;
import com.eightbitforest.delta.utils.G;

/**
 * Created by osumf on 8/19/2015.
 */
public abstract class Updates {

    public Updates()
    {
        create();
        G.i.updateThese.add(this);
    }

    public void create() {}
    public void update(float deltaTime) {}
    public void render(SpriteBatch batch) {}
    public void dispose() {}
}
