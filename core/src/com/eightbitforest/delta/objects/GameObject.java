package com.eightbitforest.delta.objects;

import com.badlogic.gdx.Gdx;
import com.badlogic.gdx.graphics.Color;
import com.badlogic.gdx.graphics.Texture;
import com.badlogic.gdx.graphics.g2d.Sprite;
import com.badlogic.gdx.graphics.g2d.SpriteBatch;
import com.eightbitforest.delta.utils.Globals;

/**
 * Created by osumf on 8/17/2015.
 */
public abstract class GameObject extends Updates {

    public Sprite sprite;
    public Texture getTexture() { return sprite.getTexture(); }

    public GameObject(String image)
    {
        super();
        if (image != null)
            sprite = new Sprite(new Texture(Gdx.files.internal(image)));
    }

    @Override
    public void dispose() {
        if (sprite != null)
            getTexture().dispose();
    }
}
