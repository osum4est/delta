package com.eightbitforest.delta.objects.base;

import com.badlogic.gdx.graphics.Color;
import com.badlogic.gdx.math.MathUtils;
import com.badlogic.gdx.physics.box2d.Body;
import com.badlogic.gdx.scenes.scene2d.Actor;
import com.badlogic.gdx.utils.JsonValue;
import com.eightbitforest.delta.level.Level;
import com.eightbitforest.delta.views.MainGame;

/**
 * Only uses Actor for updating and color
 */
public abstract class GameObject extends Actor {
    protected Body body;
    private Level level;
    private MainGame game;

    private int id;

    public GameObject(Level level, int id) {
        this(level, id, true);
    }

    public GameObject(Level level, int id, boolean createBody) {
        this.id = id;
        this.level = level;
        this.game = MainGame.getInstance();

        if (createBody)
            setBody(new BodyBuilder());
    }

    public GameObject(Level level, int id, Color color) {
        this(level, id);
        setColor(color);
    }

    public GameObject(Level level, int id, float x, float y) {
        this(level, id);
        setPosition(x, y);
    }

    public GameObject(Level level, int id, float x, float y, Color color) {
        this(level, id, x, y, color, true);
    }

    public GameObject(Level level, int id, float x, float y, Color color, boolean createBody) {
        this(level, id, createBody);
        setColor(color);
        setPosition(x, y);
    }

    @Override
    public float getX() {
        return body.getPosition().x;
    }

    @Override
    public void setX(float x) {
        if (body != null)
            body.setTransform(x, body.getPosition().y, body.getAngle());
        super.setX(x);
    }

    @Override
    public float getY() {
        return body.getPosition().y;
    }

    @Override
    public void setY(float y) {
        if (body != null)
            body.setTransform(body.getPosition().x, y, body.getAngle());
        super.setY(y);
    }

    @Override
    public void setPosition(float x, float y) {
        setX(x);
        setY(y);
    }

    @Override
    public void setRotation(float degrees) {
        body.setTransform(getX(), getY(), degrees * MathUtils.radiansToDegrees);
    }

    public int getId() {
        return id;
    }

    public void setProperties(JsonValue json) {
        if (json.has("x")) {
            setX(json.getFloat("x"));
        }
        if (json.has("y")) {
            setY(json.getFloat("y"));
        }
    }

    protected void setBody(BodyBuilder body) {
        this.body = body.createBody(level, this, id);
        this.body.setTransform(super.getX(), super.getY(), 0);
        setupShape(body.getShapeBuilder());
    }

    protected abstract void setupShape(ShapeBuilder shape);

    protected Level getLevel() {
        return level;
    }

    protected MainGame getGame() {
        return game;
    }

    public void onCollideEnter(GameObject other) {
    }

    public void onCollideExit(GameObject other) {
    }

    public Body getBody() {
        return body;
    }
}
