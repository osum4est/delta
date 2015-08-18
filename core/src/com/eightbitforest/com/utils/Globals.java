package com.eightbitforest.com.utils;

import com.badlogic.gdx.graphics.Color;
import com.badlogic.gdx.graphics.g2d.PolygonSprite;
import com.badlogic.gdx.graphics.g2d.SpriteBatch;
import com.badlogic.gdx.graphics.glutils.ShapeRenderer;
import com.badlogic.gdx.math.MathUtils;
import com.badlogic.gdx.math.Matrix4;
import com.badlogic.gdx.math.Vector2;
import com.badlogic.gdx.physics.box2d.Box2DDebugRenderer;
import com.badlogic.gdx.physics.box2d.PolygonShape;
import com.badlogic.gdx.physics.box2d.World;
import com.badlogic.gdx.utils.Array;
import com.eightbitforest.com.DeltaCamera;
import com.eightbitforest.com.objects.GameObject;
import com.eightbitforest.com.objects.Player;


/**
 * Created by osumf on 8/16/2015.
 */
public class Globals {
    public static Globals i = new Globals();

    public static final int CAMERA_SIZE = 30;
    public static final float TRIANGLE_HEIGHT = 1f;

    public SpriteBatch batch;
    public ShapeRenderer shapeRenderer;

    public GameHandler gameHandler;
    public DeltaCamera camera;
    public Array<GameObject> gameObjects;
    public World world;
    public Box2DDebugRenderer debugRenderer;


    public Player player;

    public void init()
    {
        batch = new SpriteBatch();
        shapeRenderer = new ShapeRenderer();

        gameHandler = new GameHandler();
        camera = new DeltaCamera();
        gameObjects = new Array<GameObject>();
        world = new World(Vector2.Zero, false);
        debugRenderer = new Box2DDebugRenderer();

        player = new Player();
    }

    public PolygonShape getTriangleShape(float height)
    {
        float side = (2 / (float)Math.sqrt(3)) * height;

        PolygonShape shape = new PolygonShape();
        shape.set(new Vector2[]{
                new Vector2(-side / 2, -height / 3),
                new Vector2(side / 2, -height / 3),
                new Vector2(0, height / 3 * 2)});

        return shape;
    }

    public void drawTriangle(float height, Color color)
    {
        shapeRenderer.setProjectionMatrix(camera.combined);
        float side = (2 / (float) Math.sqrt(3)) * height;

        shapeRenderer.begin(ShapeRenderer.ShapeType.Filled);
        shapeRenderer.setColor(color);
        shapeRenderer.identity();
        shapeRenderer.translate(player.body.getPosition().x, player.body.getPosition().y, 0);
        shapeRenderer.rotate(0, 0, 1, player.body.getAngle() * MathUtils.radiansToDegrees);
        shapeRenderer.triangle(-side / 2, -height / 3, side / 2, -height / 3, 0, height / 3 * 2);
        shapeRenderer.end();
    }

}
