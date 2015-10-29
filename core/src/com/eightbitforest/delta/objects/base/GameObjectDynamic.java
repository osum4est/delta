package com.eightbitforest.delta.objects.base;

import com.badlogic.gdx.physics.box2d.Body;
import com.badlogic.gdx.physics.box2d.BodyDef;
import com.badlogic.gdx.physics.box2d.FixtureDef;
import com.eightbitforest.delta.utils.G;

/**
 * Created by osumf on 8/17/2015.
 */
public abstract class GameObjectDynamic extends GameObject {

    public Body body;

    abstract Body getBody(BodyDef bdef, FixtureDef fdef);

    public GameObjectDynamic(String image, boolean setBody)
    {
        super(image);
        if (setBody)
            body = getBody(new BodyDef(), new FixtureDef());
    }

    public GameObjectDynamic(String image)
    {
        super(image);
        body = getBody(new BodyDef(), new FixtureDef());
    }

    public void onCollide(GameObjectDynamic other) { }

    @Override
    public void dispose() {
        G.i.gameHandler.removeGameObject(this);
        super.dispose();
    }
}
