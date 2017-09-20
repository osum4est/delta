package com.eightbitforest.delta.objects.base;

import com.badlogic.gdx.physics.box2d.PolygonShape;
import com.badlogic.gdx.physics.box2d.Shape;
import com.eightbitforest.delta.utils.Constants;
import com.eightbitforest.delta.utils.Utils;

public class ShapeBuilder {
    private float[] vertices;

    public ShapeBuilder setAsRect(float width, float height) {
        vertices = new float[]{
                -width / 2, -height / 3,
                -width / 2, height / 3 * 2,
                width / 2, height / 3 * 2,
                width / 2, -height / 3
        };

        return this;
    }

    public ShapeBuilder setAsTriangle() {
        return setAsTriangle(Constants.TRIANGLE_SIDE);
    }

    public ShapeBuilder setAsTriangle(float triangleSize) {
        float height = Utils.getTriangleHeight(triangleSize);
        vertices = new float[]{
                -triangleSize / 2, -height / 3,
                triangleSize / 2, -height / 3,
                0, height / 3 * 2
        };

        return this;
    }

    public ShapeBuilder setAsHalfTriangle() {
        vertices = new float[]{
                0, -Constants.TRIANGLE_HEIGHT / 3,
                Constants.TRIANGLE_SIDE / 2, Constants.TRIANGLE_HEIGHT / 3 * 2,
                Constants.TRIANGLE_SIDE / 2, -Constants.TRIANGLE_HEIGHT / 3
        };

        return this;
    }

    public ShapeBuilder setAsHalfTriangleLong() {
        vertices = new float[]{
                -Constants.TRIANGLE_SIDE / 2, -Constants.TRIANGLE_HEIGHT / 3,
                0, Constants.TRIANGLE_HEIGHT / 3 * 2,
                Constants.TRIANGLE_SIDE / 2, Constants.TRIANGLE_HEIGHT / 3 * 2,
                Constants.TRIANGLE_SIDE / 2, -Constants.TRIANGLE_HEIGHT / 3
        };

        return this;
    }

    public ShapeBuilder flip(boolean x, boolean y) {
        if (vertices != null) {
            for (int i = 0; i < vertices.length; i += 2) {
                if (x)
                    vertices[i] *= -1;
                if (y) {
                    vertices[i + 1] *= -1;
                    vertices[i + 1] += Constants.TRIANGLE_HEIGHT / 3;
                }
            }
        }

        return this;
    }

    public Shape createShape() {
        PolygonShape shape = new PolygonShape();
        if (vertices == null)
            setAsTriangle();

        shape.set(vertices);
        return shape;
    }

    public float[] getVertices() {
        return vertices;
    }
}
