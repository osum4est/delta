package com.eightbitforest.delta.objects.base;

import com.badlogic.gdx.graphics.g2d.SpriteBatch;
import com.badlogic.gdx.physics.box2d.*;
import com.eightbitforest.delta.utils.BodyData;
import com.eightbitforest.delta.utils.G;
import com.eightbitforest.delta.utils.ObjectType;

public class PlayerDespawnCollider extends GameObjectDynamic{

    public PlayerDespawnCollider() {
        super(ObjectType.DESPAWN_CIRCLE,
                new BodyBuilder()
                        .setBodyType(BodyDef.BodyType.KinematicBody)
                        .setShape(createCircleShape())
                        .setSensor(true));
    }

    private static Shape createCircleShape() {
        CircleShape shape = new CircleShape();
        shape.setRadius(G.i.DESPAWN_RADIUS);
        return shape;
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
