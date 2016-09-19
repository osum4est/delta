package com.eightbitforest.delta.objects;

import com.badlogic.gdx.graphics.Color;
import com.badlogic.gdx.math.Vector2;
import com.badlogic.gdx.physics.box2d.BodyDef;
import com.eightbitforest.delta.objects.base.BodyBuilder;
import com.eightbitforest.delta.objects.base.GameObjectDynamicTriangle;
import com.eightbitforest.delta.utils.ObjectType;

/**
 * Created by osum4est on 9/18/16.
 */
public class ObjectPlanetPart extends GameObjectDynamicTriangle {

    public ObjectPlanetPart(Vector2 pos, float rotation)
    {
        super(ObjectType.PLANET_PART, new BodyBuilder()
                .setBodyType(BodyDef.BodyType.KinematicBody)
                .setPosition(pos)
                .setRotation((float)Math.toRadians(rotation)));
    }

    @Override
    public Color getColor() {
        return Color.FIREBRICK;
    }


}
