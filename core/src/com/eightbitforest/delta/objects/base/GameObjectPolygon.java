package com.eightbitforest.delta.objects.base;

import com.badlogic.gdx.graphics.Color;
import com.badlogic.gdx.graphics.Pixmap;
import com.badlogic.gdx.graphics.Texture;
import com.badlogic.gdx.graphics.g2d.PolygonRegion;
import com.badlogic.gdx.graphics.g2d.PolygonSprite;
import com.badlogic.gdx.graphics.g2d.PolygonSpriteBatch;
import com.badlogic.gdx.graphics.g2d.TextureRegion;
import com.badlogic.gdx.math.EarClippingTriangulator;
import com.eightbitforest.delta.level.Level;

public abstract class GameObjectPolygon extends GameObject {
    private static boolean drawing = false;
    private PolygonSprite[] polygonSprites;

    public GameObjectPolygon(Level level, int id) {
        super(level, id);
    }

    public GameObjectPolygon(Level level, int id, boolean createBody) {
        super(level, id, createBody);
    }

    public GameObjectPolygon(Level level, int id, Color color) {
        super(level, id, color);
    }

    public GameObjectPolygon(Level level, int id, float x, float y) {
        super(level, id, x, y);
    }

    public GameObjectPolygon(Level level, int id, float x, float y, Color color) {
        super(level, id, x, y, color);
    }

    public GameObjectPolygon(Level level, int id, float x, float y, Color color, boolean createBody) {
        super(level, id, x, y, color, createBody);
    }

    public GameObjectPolygon(Level level, int id, float x, float y, Color color, float triangleSize) {
        super(level, id, false);
        setBody(new BodyBuilder().setShape(new ShapeBuilder().setAsTriangle(triangleSize)));
        body.setTransform(x, y, 0);
        setColor(color);
    }

    @Override
    public void drawShape(PolygonSpriteBatch batch) {
        for (PolygonSprite polygonSprite : polygonSprites) {
            polygonSprite.setPosition(getX(), getY());
            polygonSprite.setColor(getColor());
            polygonSprite.setRotation(getRotation());
            polygonSprite.setScale(getScaleX());
            polygonSprite.draw(batch);
        }
    }

    @Override
    protected void setupShape(ShapeBuilder shape) {
        float[][] vertices = shape.getVertices();

        Pixmap pixmap = new Pixmap(1, 1, Pixmap.Format.RGBA8888);
        pixmap.setColor(1, 1, 1, 1);
        pixmap.fill();

        polygonSprites = new PolygonSprite[vertices.length];
        for (int i = 0; i < polygonSprites.length; i++) {
            polygonSprites[i] = new PolygonSprite(new PolygonRegion(new TextureRegion(new Texture(pixmap)),
                    vertices[i], new EarClippingTriangulator().computeTriangles(vertices[i]).toArray()));
            polygonSprites[i].setOrigin(0, 0);
        }
    }

    // 1000
    // 1000
    // 1000
}
