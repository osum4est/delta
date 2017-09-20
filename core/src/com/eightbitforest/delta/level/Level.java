package com.eightbitforest.delta.level;

import com.badlogic.gdx.Gdx;
import com.badlogic.gdx.math.Vector2;
import com.badlogic.gdx.physics.box2d.World;
import com.badlogic.gdx.scenes.scene2d.InputEvent;
import com.badlogic.gdx.scenes.scene2d.InputListener;
import com.badlogic.gdx.scenes.scene2d.Stage;
import com.badlogic.gdx.utils.viewport.ExtendViewport;
import com.eightbitforest.delta.objects.Player;
import com.eightbitforest.delta.objects.base.GameObjectPolygon;
import com.eightbitforest.delta.utils.CollisionHandler;
import com.eightbitforest.delta.utils.Constants;

import java.util.ArrayList;

public class Level extends Stage {
    private World world;
    private CollisionHandler collisionHandler;

    private Player player;
    private ArrayList<GameObjectPolygon> objects;

    public Level() {
        super(new ExtendViewport(Constants.VIEWPORT_WIDTH / Constants.PPM, Constants.VIEWPORT_HEIGHT / Constants.PPM));

        world = new World(
                new Vector2(0, 0),
                false
        );
        collisionHandler = new CollisionHandler();
        world.setContactListener(collisionHandler);

        this.objects = new ArrayList<GameObjectPolygon>();
        this.player = new Player(this, 0f, 0f);
        addObject(this.player);

        addListener(new InputListener() {
            float prevX = 0;
            float prevY = 0;
            @Override
            public boolean touchDown(InputEvent event, float x, float y, int pointer, int button) {
                player.startMoving();
                prevX = Gdx.input.getX();
                prevY = Gdx.input.getY();
                return true;
            }

            @Override
            public void touchDragged(InputEvent event, float x, float y, int pointer) {
                if (prevX != Gdx.input.getX())
                    player.turn(((Gdx.input.getX() - prevX) / (float) Gdx.graphics.getWidth()));
                prevX = Gdx.input.getX();
                prevY = Gdx.input.getY();
            }

            @Override
            public void touchUp(InputEvent event, float x, float y, int pointer, int button) {
                player.stopMoving();
            }
        });
    }

    @Override
    public void act(float delta) {
        super.act(delta);
        world.step(1 / 60f, 8, 3);
        collisionHandler.step();
    }

    public World getWorld() {
        return world;
    }

    public Player getPlayer() {
        return player;
    }

    public void addObject(GameObjectPolygon object) {
        addActor(object);
        objects.add(object);
    }
}
