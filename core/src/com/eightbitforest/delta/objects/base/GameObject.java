package com.eightbitforest.delta.objects.base;

import com.badlogic.gdx.Gdx;
import com.badlogic.gdx.graphics.Texture;
import com.badlogic.gdx.graphics.g2d.Sprite;
import com.eightbitforest.delta.utils.GameRegistry;
import com.eightbitforest.delta.utils.interfaces.IUpdates;


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
    }

    public abstract int getId();

    @Override
    public void dispose() {
        GameRegistry.unregisterObject(this);
        if (sprite != null)
            getTexture().dispose();
    }
}
