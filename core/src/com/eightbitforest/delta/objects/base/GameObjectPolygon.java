package com.eightbitforest.delta.objects.base;

import com.badlogic.gdx.graphics.Color;
import com.badlogic.gdx.graphics.Pixmap;
import com.badlogic.gdx.graphics.Texture;
import com.badlogic.gdx.graphics.g2d.*;
import com.badlogic.gdx.math.EarClippingTriangulator;
import com.badlogic.gdx.math.MathUtils;
import com.badlogic.gdx.physics.box2d.Body;
import com.badlogic.gdx.scenes.scene2d.Actor;
import com.badlogic.gdx.utils.JsonValue;
import com.eightbitforest.delta.level.Level;

/**
 * Only uses Actor for updating and color
 */
public abstract class GameObjectPolygon extends Actor {
    private PolygonSpriteBatch polygonSpriteBatch;
    private PolygonSprite[] polygonSprites;

    protected Body body;
    private Level level;

    private int id;

    public GameObjectPolygon(Level level, int id) {
        this(level, id, true);
    }

    public GameObjectPolygon(Level level, int id, boolean createBody) {
        polygonSpriteBatch = new PolygonSpriteBatch();
        this.id = id;
        this.level = level;

        if (createBody)
            setBody(new BodyBuilder());
    }

    public GameObjectPolygon(Level level, int id, Color color) {
        this(level, id);
        setColor(color);
    }

    public GameObjectPolygon(Level level, int id, float x, float y) {
        this(level, id);
        setPosition(x, y);
    }

    public GameObjectPolygon(Level level, int id, float x, float y, Color color) {
        this(level, id, x, y, color, true);
    }

    public GameObjectPolygon(Level level, int id, float x, float y, Color color, boolean createBody) {
        this(level, id, createBody);
        setColor(color);
        setPosition(x, y);
    }

    public GameObjectPolygon(Level level, int id, float x, float y, Color color, float triangleSize) {
        this(level, id, false);
        setBody(new BodyBuilder().setShape(new ShapeBuilder().setAsTriangle(triangleSize)));
        body.setTransform(x, y, 0);
        setColor(color);
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

    @Override
    public void draw(Batch batch, float parentAlpha) {
        batch.end();

        polygonSpriteBatch.setProjectionMatrix(batch.getProjectionMatrix());
        polygonSpriteBatch.setTransformMatrix(batch.getTransformMatrix());
        polygonSpriteBatch.begin();

        for (PolygonSprite polygonSprite : polygonSprites) {
            polygonSprite.setPosition(getX(), getY());
            polygonSprite.setColor(getColor());
            polygonSprite.setRotation(body.getAngle() * MathUtils.radiansToDegrees);
            polygonSprite.draw(polygonSpriteBatch);
        }

        polygonSpriteBatch.end();
        batch.begin();
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
        float[][] vertices = body.getShapeVertices();

        Pixmap pixmap = new Pixmap(1, 1, Pixmap.Format.RGBA8888);
        pixmap.setColor(getColor());
        pixmap.fill();

        polygonSprites = new PolygonSprite[vertices.length];
        for (int i = 0; i < polygonSprites.length; i++) {
            polygonSprites[i] = new PolygonSprite(new PolygonRegion(new TextureRegion(new Texture(pixmap)),
                    vertices[i], new EarClippingTriangulator().computeTriangles(vertices[i]).toArray()));
            polygonSprites[i].setOrigin(0, 0);
        }
    }

    protected Level getLevel() {
        return level;
    }

    public void onCollideEnter(GameObjectPolygon other) {
    }

    public void onCollideExit(GameObjectPolygon other) {
    }

    public Body getBody() {
        return body;
    }
}
