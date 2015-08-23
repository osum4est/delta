package com.eightbitforest.delta.utils;

import com.badlogic.gdx.graphics.Color;
import com.badlogic.gdx.graphics.g2d.SpriteBatch;
import com.badlogic.gdx.graphics.glutils.ShapeRenderer;
import com.badlogic.gdx.math.Vector2;
import com.badlogic.gdx.physics.box2d.Box2DDebugRenderer;
import com.badlogic.gdx.physics.box2d.World;
import com.badlogic.gdx.utils.Array;
import com.eightbitforest.delta.DeltaCamera;
import com.eightbitforest.delta.objects.ObjectAsteroid;
import com.eightbitforest.delta.objects.Player;
import com.eightbitforest.delta.objects.Updates;


/**
 * Created by osumf on 8/16/2015.
 */
public class Globals {
    public static Globals i = new Globals();

    public final int CAMERA_SIZE = 2;
    public final float TRIANGLE_HEIGHT = 1f;
    public final int SPAWN_SQUARE_SIZE = 100;

    public SpriteBatch batch;
    public ShapeRenderer shapeRenderer;

    public GameHandler gameHandler;
    public DeltaCamera camera;
    public Array<Updates> updateThese;
    public World world;
    public Box2DDebugRenderer debugRenderer;
    public Array<Class<? extends ISpawnable>> spawnables;

    public Player player;

    public void init()
    {
        batch = new SpriteBatch();
        shapeRenderer = new ShapeRenderer();

        gameHandler = new GameHandler();
        camera = new DeltaCamera();
        updateThese = new Array<Updates>();
        world = new World(Vector2.Zero, false);
        debugRenderer = new Box2DDebugRenderer();
        spawnables = new Array<Class<? extends ISpawnable>>();

        player = new Player();
        new ObjectAsteroid();
        new ObjectSpawner();
    }
}
