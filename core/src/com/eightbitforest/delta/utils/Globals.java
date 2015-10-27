package com.eightbitforest.delta.utils;

import com.badlogic.gdx.graphics.g2d.SpriteBatch;
import com.badlogic.gdx.graphics.glutils.ShapeRenderer;
import com.badlogic.gdx.math.Vector2;
import com.badlogic.gdx.physics.box2d.Box2DDebugRenderer;
import com.badlogic.gdx.physics.box2d.World;
import com.badlogic.gdx.utils.Array;
import com.eightbitforest.delta.DeltaCamera;
import com.eightbitforest.delta.objects.GameObjectDynamic;
import com.eightbitforest.delta.objects.ObjectAsteroid;
import com.eightbitforest.delta.objects.Player;
import org.reflections.Reflections;

import java.util.Set;


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

        updateThese = new Array<IUpdates>();
        inputThese = new Array<ITouchInput>();
        removeThese = new Array<GameObjectDynamic>();

        world = new World(Vector2.Zero, false);
        debugRenderer = new Box2DDebugRenderer();

        spawnables = new Array<Class<? extends ISpawnable>>();
        initSpawnables();

        world.setContactListener(new CollisionHandler());

        player = new Player();
        new ObjectAsteroid();
        new ObjectSpawner();


    }

    public void initSpawnables() {
        Reflections reflections = new Reflections("com.eightbitforest.delta");
        Set<Class<? extends ISpawnable>> spawnablesSet = reflections.getSubTypesOf(ISpawnable.class);

        for (Class<? extends ISpawnable> spawn : spawnablesSet) {
            System.out.println(spawn.getName());
            spawnables.add(spawn);
        }
    }
}
