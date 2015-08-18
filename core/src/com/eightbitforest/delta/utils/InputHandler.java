package com.eightbitforest.delta.utils;

import com.badlogic.gdx.Gdx;
import com.badlogic.gdx.InputProcessor;

/**
 * Created by osumf on 8/17/2015.
 */
public class InputHandler implements InputProcessor {

    @Override
    public boolean keyDown(int keycode) {
        return false;
    }

    @Override
    public boolean keyUp(int keycode) {
        return false;
    }

    @Override
    public boolean keyTyped(char character) {
        return false;
    }

    @Override
    public boolean touchDown(int screenX, int screenY, int pointer, int button) {

        Globals.i.player.force = Globals.i.player.thrust;
        return true;
    }

    @Override
    public boolean touchUp(int screenX, int screenY, int pointer, int button) {
        Globals.i.player.force = 0;

        return true;
    }

    @Override
    public boolean touchDragged(int screenX, int screenY, int pointer) {

        Globals.i.player.torque = Gdx.input.getDeltaX() * -Globals.i.player.turnSpeed;
        return true;
    }

    @Override
    public boolean mouseMoved(int screenX, int screenY) {
        return false;
    }

    @Override
    public boolean scrolled(int amount) {
        return false;
    }
}
