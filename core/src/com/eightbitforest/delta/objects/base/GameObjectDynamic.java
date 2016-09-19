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

    public Body getBody() { return body; }

    public GameObjectDynamic(int id, String image, boolean setBody)
    {
        super(id, image);
        if (setBody)
            body = getBody();
    }

    public GameObjectDynamic(int id, String image)
    {
        super(id, image);
        body = getBody();
    }

    public GameObjectDynamic(int id, BodyBuilder body)
    {
        super(id, null);
        this.body = body.createBody(this, id);
    }

    public void onCollideEnter(GameObjectDynamic other) { }
    public void onCollideExit(GameObjectDynamic other) { }

    @Override
    public void dispose() {
        G.i.gameHandler.removeGameObject(this);
        super.dispose();
    }
}
