package com.eightbitforest.delta.objects.base;

import com.badlogic.gdx.Gdx;
import com.badlogic.gdx.graphics.Color;
import com.badlogic.gdx.graphics.g2d.Batch;
import com.badlogic.gdx.graphics.glutils.ShapeRenderer;
import com.badlogic.gdx.math.MathUtils;
import com.badlogic.gdx.physics.box2d.Body;
import com.badlogic.gdx.scenes.scene2d.Actor;
import com.eightbitforest.delta.level.Level;
import com.eightbitforest.delta.utils.Constants;

/**
 * Only uses Actor for updating and color
 */
public abstract class GameObject extends Actor {
    protected ShapeRenderer renderer;
    protected Body body;
    private Level level;

    private int id;

    public GameObject(Level level, int id)
    {
        body = new BodyBuilder().createBody(level, this, id);
        renderer = new ShapeRenderer();
        this.id = id;
        this.level = level;
    }

    public GameObject(Level level, int id, Color color) {
        this(level, id);
        setColor(color);
    }

    public GameObject(Level level, int id, float x, float y) {
        this(level, id);
        body.setTransform(x, y, 0);
    }

    public GameObject(Level level, int id, float x, float y, Color color) {
        this(level, id);
        body.setTransform(x, y, 0);
        setColor(color);
    }

    @Override
    public float getX() {
        return body.getPosition().x;
    }

    @Override
    public void setX(float x) {
        body.getPosition().x = x;
    }

    @Override
    public float getY() {
        return body.getPosition().y;
    }

    @Override
    public void setY(float y) {
        body.getPosition().y = y;
    }

    @Override
    public void draw(Batch batch, float parentAlpha) {
        batch.end();

        renderer.setProjectionMatrix(batch.getProjectionMatrix());
        renderer.setTransformMatrix(batch.getTransformMatrix());

        renderer.translate(getX(), getY(), 0);
        renderer.setColor(getColor());
        renderer.rotate(0, 0, 1, body.getAngle() * MathUtils.radiansToDegrees);

        renderer.begin(ShapeRenderer.ShapeType.Filled);
        renderer.triangle(
                -Constants.TRIANGLE_SIDE / 2, -Constants.TRIANGLE_HEIGHT / 3,
                Constants.TRIANGLE_SIDE / 2, -Constants.TRIANGLE_HEIGHT / 3,
                0, Constants.TRIANGLE_HEIGHT / 3 * 2
        );
        renderer.end();

        batch.begin();
    }

    public int getId() {
        return id;
    }
}
