import { IoRepeatSharp } from 'react-icons/io5';

export const Repeat = ({ isOnRepeat, setIsOnRepeat }) => {
    return(
        <div className="repeat-container">
            <button onClick={() => setIsOnRepeat(prev => !prev)} className={isOnRepeat ? "controls-button active-repeat" : 'controls-button'}>
                <IoRepeatSharp className={ isOnRepeat? 'repeat-icon onrepeat' : "repeat-icon"}/>
            </button>
        </div>
    );
}