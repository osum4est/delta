package com.eightbitforest.delta.objects.base;

import com.badlogic.gdx.graphics.g2d.SpriteBatch;
import com.badlogic.gdx.physics.box2d.*;
import com.eightbitforest.delta.utils.BodyData;
import com.eightbitforest.delta.utils.G;
import com.eightbitforest.delta.utils.ObjectType;

public class PlayerDespawnCollider extends GameObjectDynamic{

    public PlayerDespawnCollider() {
        super(ObjectType.DESPAWN_CIRCLE, null);
    }

    @Override
    Body getBody(BodyDef bdef, FixtureDef fdef) {
        Body body;

        bdef.type = BodyDef.BodyType.KinematicBody;
        bdef.position.set(0, 0);

        CircleShape shape = new CircleShape();
        shape.setRadius(G.i.DESPAWN_RADIUS);

        fdef.shape = shape;
        fdef.filter.maskBits = G.i.MASK_ALL;
        fdef.filter.categoryBits = G.i.CATEGORY_ALL;
        fdef.isSensor = true;

        body = G.i.world.createBody(bdef);
        body.createFixture(fdef);
        shape.dispose();

        body.setUserData(new BodyData(this, id));

        return body;
    }

    @Override
    public void update(float deltaTime) {
        body.setTransform(G.i.player.body.getPosition(), 0);
    }

    @Override
    public void onCollideExit(GameObjectDynamic other) {
        other.dispose();
    }

    @Override
    public void render(SpriteBatch batch) {

    }
}
