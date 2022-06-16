import React from 'react';

export default function RadioButtons() {
    const [radioArray] = React.useState(['option1', 'option2', 'option3']);
    const [radio, setRadio] = React.useState<any | null>(null);

    React.useEffect(() => console.info("radio", radio), [radio]);

    const handleChange = (e: any) => {
        const {name, value} = e.target;

        setRadio({[name]: value});
    };

    return (<React.Fragment>
        <form>
            {radioArray.map((radio, index) =>
                (<>
                    <br key={"br"+index}/>
                    <input
                        key={index}
                        type="radio"
                        name="options"
                        id={`input-${index}`} value={radio} onChange={handleChange}/>
                    <label key={"lbl"+index} htmlFor={`input-${index}`}>{radio}</label>
                </>))
            }
        </form>
    </React.Fragment>)
}
