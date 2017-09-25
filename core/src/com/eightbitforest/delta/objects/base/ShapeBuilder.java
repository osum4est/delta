package com.eightbitforest.delta.objects.base;

import com.badlogic.gdx.physics.box2d.CircleShape;
import com.badlogic.gdx.physics.box2d.PolygonShape;
import com.badlogic.gdx.physics.box2d.Shape;
import com.eightbitforest.delta.utils.Constants;
import com.eightbitforest.delta.utils.Utils;

public class ShapeBuilder {
    private Type type = Type.POLYGON;
    private float[][] vertices;
    private float radius;

    public ShapeBuilder setAsRect(float width, float height) {
        vertices = new float[][]{new float[]{
                -width / 2, -height / 3,
                -width / 2, height / 3 * 2,
                width / 2, height / 3 * 2,
                width / 2, -height / 3
        }};

        return this;
    }

    public ShapeBuilder setAsTriangle() {
        return setAsTriangle(Constants.TRIANGLE_SIDE);
    }

    public ShapeBuilder setAsTriangle(float triangleSize) {
        float height = Utils.getTriangleHeight(triangleSize);
        vertices = new float[][]{new float[]{
                -triangleSize / 2, -height / 3,
                triangleSize / 2, -height / 3,
                0, height / 3 * 2
        }};

        return this;
    }

    public ShapeBuilder setAsHalfTriangle() {
        vertices = new float[][]{new float[]{
                0, -Constants.TRIANGLE_HEIGHT / 3,
                Constants.TRIANGLE_SIDE / 2, Constants.TRIANGLE_HEIGHT / 3 * 2,
                Constants.TRIANGLE_SIDE / 2, -Constants.TRIANGLE_HEIGHT / 3
        }};

        return this;
    }

    public ShapeBuilder setAsHalfTriangleLong() {
        vertices = new float[][]{new float[]{
                -Constants.TRIANGLE_SIDE / 2, -Constants.TRIANGLE_HEIGHT / 3,
                0, Constants.TRIANGLE_HEIGHT / 3 * 2,
                Constants.TRIANGLE_SIDE / 2, Constants.TRIANGLE_HEIGHT / 3 * 2,
                Constants.TRIANGLE_SIDE / 2, -Constants.TRIANGLE_HEIGHT / 3
        }};

        return this;
    }

    public ShapeBuilder setAsInvertedTriangle() {
        vertices = new float[][]{new float[]{
                -Constants.TRIANGLE_SIDE / 2, -Constants.TRIANGLE_HEIGHT / 3,
                -Constants.TRIANGLE_SIDE / 2, Constants.TRIANGLE_HEIGHT / 3 * 2,
                0, Constants.TRIANGLE_HEIGHT / 3 * 2
        }, new float[]{
                Constants.TRIANGLE_SIDE / 2, -Constants.TRIANGLE_HEIGHT / 3,
                0, Constants.TRIANGLE_HEIGHT / 3 * 2,
                Constants.TRIANGLE_SIDE / 2, Constants.TRIANGLE_HEIGHT / 3 * 2,

        }};
        return this;
    }

    public ShapeBuilder flip(boolean x, boolean y) {
        if (vertices != null) {
            for (int s = 0; s < vertices.length; s += 2) {
                for (int i = 0; i < vertices[s].length; i += 2) {
                    if (x)
                        vertices[s][i] *= -1;
                    if (y) {
                        vertices[s][i + 1] *= -1;
                        vertices[s][i + 1] += Constants.TRIANGLE_HEIGHT / 3;
                    }
                }
            }
        }

        return this;
    }

    public ShapeBuilder setAsCircle(float radius) {
        type = Type.CIRCLE;
        this.radius = radius;
        return this;
    }

    public Shape[] createShapes() {
        if (vertices == null && type == Type.POLYGON)
            setAsTriangle();

        Shape[] shapes = null;

        if (type == Type.POLYGON) {
            shapes = new PolygonShape[vertices.length];
            for (int i = 0; i < shapes.length; i++) {
                shapes[i] = new PolygonShape();
                ((PolygonShape) shapes[i]).set(vertices[i]);
            }
        } else if (type == Type.CIRCLE) {
            shapes = new CircleShape[1];
            CircleShape circle = new CircleShape();
            circle.setRadius(radius);
            shapes[0] = circle;
        }

        return shapes;
    }

    public float[][] getVertices() {
        return vertices;
    }

    public Type getType() {
        return type;
    }

    public float getRadius() {
        return radius;
    }

    enum Type {
        POLYGON,
        CIRCLE
    }
}
