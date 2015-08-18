package com.eightbitforest.delta.objects;

import com.badlogic.gdx.Gdx;
import com.badlogic.gdx.graphics.Texture;
import com.badlogic.gdx.graphics.g2d.Sprite;
import com.badlogic.gdx.graphics.g2d.SpriteBatch;
import com.eightbitforest.delta.utils.Globals;

/**
 * Created by osumf on 8/17/2015.
 */
public abstract class GameObject {

    public Sprite sprite;
    public Texture getTexture() { return sprite.getTexture(); }

    public GameObject()
    {
        this("images/triangle.png");
    }

    public GameObject(String image)
    {
        create();
        sprite = new Sprite(new Texture(Gdx.files.internal(image)));
        Globals.i.gameObjects.add(this);
    }

    public void create() {}
    public void update(float deltaTime) {}
    public void render(SpriteBatch batch) {}

    public void dispose() {
        getTexture().dispose();
    }
}
