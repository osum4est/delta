package com.eightbitforest.delta.utils;

import com.badlogic.gdx.InputProcessor;
import com.eightbitforest.delta.utils.interfaces.ITouchInput;

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

        for (ITouchInput touch : G.i.inputThese)
            touch.touchDown(screenX, screenY, pointer, button);
        return true;
    }

    @Override
    public boolean touchUp(int screenX, int screenY, int pointer, int button) {
        for (ITouchInput touch : G.i.inputThese)
            touch.touchUp(screenX, screenY, pointer, button);
        return true;
    }

    @Override
    public boolean touchDragged(int screenX, int screenY, int pointer) {
        for (ITouchInput touch : G.i.inputThese)
            touch.touchDragged(screenX, screenY, pointer);
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
