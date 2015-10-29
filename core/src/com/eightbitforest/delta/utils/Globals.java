package com.eightbitforest.delta.utils;

import com.badlogic.gdx.graphics.g2d.SpriteBatch;
import com.badlogic.gdx.graphics.glutils.ShapeRenderer;
import com.badlogic.gdx.math.Vector2;
import com.badlogic.gdx.physics.box2d.Box2DDebugRenderer;
import com.badlogic.gdx.physics.box2d.World;
import com.badlogic.gdx.utils.Array;
import com.eightbitforest.delta.objects.base.GameObjectDynamic;
import com.eightbitforest.delta.objects.ObjectAsteroid;
import com.eightbitforest.delta.objects.ObjectEnemy;
import com.eightbitforest.delta.objects.Player;
import com.eightbitforest.delta.utils.interfaces.ISpawnable;
import com.eightbitforest.delta.utils.interfaces.ITouchInput;
import com.eightbitforest.delta.utils.interfaces.IUpdates;

public class Globals {
    public static Globals i = new Globals();

    public final int CAMERA_SIZE = 30;
    public final float TRIANGLE_HEIGHT = 1f;
    public final int SPAWN_SQUARE_SIZE = 100;

    public SpriteBatch batch;
    public ShapeRenderer shapeRenderer;

    public GameHandler gameHandler;
    public DeltaCamera camera;

    public Array<IUpdates> updateThese;
    public Array<ITouchInput> inputThese;
    public Array<GameObjectDynamic> removeThese;

    public Array<Class<? extends ISpawnable>> spawnThese;

    public World world;
    public Box2DDebugRenderer debugRenderer;


    public Player player;

    public void init()
    {
        batch = new SpriteBatch();
        shapeRenderer = new ShapeRenderer();

        gameHandler = new GameHandler();
        camera = new DeltaCamera();

        updateThese = new Array<IUpdates>();
        inputThese = new Array<ITouchInput>();
        removeThese = new Array<GameObjectDynamic>();
        spawnThese = new Array<Class<? extends ISpawnable>>();

        world = new World(new Vector2(0, 0), false);
        debugRenderer = new Box2DDebugRenderer();

        world.setContactListener(new CollisionHandler());

        player = new Player();
        GameRegistry.registerObject(player);
        GameRegistry.registerObject(new ObjectSpawner());

        registerSpawnables();
    }

    public void registerSpawnables() {
        GameRegistry.registerSpawnable(ObjectAsteroid.class);
        GameRegistry.registerSpawnable(ObjectEnemy.class);
    }
}
