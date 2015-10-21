package com.eightbitforest.delta.objects;

import com.badlogic.gdx.Gdx;
import com.badlogic.gdx.graphics.Texture;
import com.badlogic.gdx.graphics.g2d.Sprite;
import com.eightbitforest.delta.utils.G;

/**
 * Created by osumf on 8/17/2015.
 */
public abstract class GameObject extends Updates {

    public Sprite sprite;
    public Texture getTexture() { return sprite.getTexture(); }

    public int id;

    public GameObject(String image)
    {
        super();
        if (image != null)
            sprite = new Sprite(new Texture(Gdx.files.internal(image)));
        id = getId();
    }

    abstract int getId();

    @Override
    public void dispose() {
        System.out.println("Disposing");
        G.i.updateThese.removeValue(this, false);
        if (sprite != null)
            getTexture().dispose();
    }
}
