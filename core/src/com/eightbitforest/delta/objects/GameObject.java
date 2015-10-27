package com.eightbitforest.delta.objects;

import com.badlogic.gdx.Gdx;
import com.badlogic.gdx.graphics.Texture;
import com.badlogic.gdx.graphics.g2d.Sprite;
import com.badlogic.gdx.graphics.g2d.SpriteBatch;
import com.eightbitforest.delta.utils.G;
import com.eightbitforest.delta.utils.IUpdates;

/**
 * Created by osumf on 8/17/2015.
 */
public abstract class GameObject implements IUpdates {

    public Sprite sprite;
    public Texture getTexture() { return sprite.getTexture(); }

    public int id;

    public GameObject(String image)
    {
        super();
        if (image != null)
            sprite = new Sprite(new Texture(Gdx.files.internal(image)));

        id = getId();
        G.i.updateThese.add(this);
    }

    abstract int getId();

    @Override
    public void update(float deltaTime) {

    }

    @Override
    public void render(SpriteBatch batch) {

    }

    @Override
    public void dispose() {
        //System.out.println("Disposing " + id);
        G.i.updateThese.removeValue(this, false);
        if (sprite != null)
            getTexture().dispose();
    }
}
