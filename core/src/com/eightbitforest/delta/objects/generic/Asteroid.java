package com.eightbitforest.delta.objects.generic;

import com.badlogic.gdx.graphics.Color;
import com.badlogic.gdx.physics.box2d.BodyDef;
import com.eightbitforest.delta.level.Level;
import com.eightbitforest.delta.objects.base.BodyBuilder;
import com.eightbitforest.delta.objects.base.GameObjectPolygon;
import com.eightbitforest.delta.objects.base.ShapeBuilder;
import com.eightbitforest.delta.utils.Constants;
import com.eightbitforest.delta.utils.Ids;
import com.eightbitforest.delta.utils.Utils;

public class Asteroid extends GameObjectPolygon {
    public Asteroid(Level level, float x, float y) {
        super(level, Ids.ASTEROID, x, y, Color.GRAY, false);
        setBody(new BodyBuilder().setBodyType(BodyDef.BodyType.KinematicBody).setShape(
                new ShapeBuilder().setAsPolygon(getVertices())
        ));
        setY(getY() + Constants.TRIANGLE_HEIGHT / 3 / 2);
        setRotation(Utils.randomRange(0, 360));
    }

    private float[] getVertices() {
        float[] vertices = new float[]{
                -Constants.TRIANGLE_SIDE / 4, Constants.TRIANGLE_HEIGHT / 2,
                Constants.TRIANGLE_SIDE / 4, Constants.TRIANGLE_HEIGHT / 2,
                Constants.TRIANGLE_SIDE / 2, 0,
                Constants.TRIANGLE_SIDE / 4, -Constants.TRIANGLE_HEIGHT / 2,
                -Constants.TRIANGLE_SIDE / 4, -Constants.TRIANGLE_HEIGHT / 2,
                -Constants.TRIANGLE_SIDE / 2, 0
        };
        return randomizeVertices(vertices);
    }

    private float[] randomizeVertices(float[] vertices) {
        for (int i = 0; i < vertices.length; i++) {
            vertices[i] = Utils.randomRange(
                    vertices[i] - Constants.ASTEROID_VARIATION / 2,
                    vertices[i] + Constants.ASTEROID_VARIATION / 2
            );
        }
        return vertices;
    }
}
