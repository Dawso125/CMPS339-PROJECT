import React, { useEffect, useState } from "react";
import { Button, Container, Flex, Space, TextInput } from "@mantine/core";
import { FormErrors, useForm } from "@mantine/form";
import { useNavigate, useParams } from "react-router-dom";
import { showNotification } from "@mantine/notifications";
import { routes } from "../../routes";
import { ParksCreateUpdateDto, ParksGetDto } from "../../constants/types";
import axios from "axios"; // Import Axios library

export const GameConsoleUpdate = () => {
  const [park, setPark] = useState<ParksGetDto | undefined>(undefined);

  const navigate = useNavigate();
  const { id } = useParams();

  const mantineForm = useForm<ParksCreateUpdateDto>({
    initialValues: park
  });

  useEffect(() => {
    fetchPark();

    async function fetchPark() {
      try {
        const response = await axios.get<ParksGetDto>(`/api/park/${id}`);

        if (response.data) {
          setPark(response.data);
          mantineForm.setValues(response.data);
          mantineForm.resetDirty();
        } else {
          showNotification({ message: "Error finding park", color: "red" });
        }
      } catch (error) {
        showNotification({ message: "Error finding park", color: "red" });
      }
    }
  }, [id]);

  const submitPark = async (values) => {
    try {
      const response = await axios.put<ParksGetDto>(`/api/park/${id}`, values);

      if (response.data) {
        showNotification({
          message: "Park successfully updated",
          color: "green"
        });
        navigate(routes.ParksListing);
      } else {
        const formErrors = response.data.errors.reduce((prev, curr) => {
          Object.assign(prev, { [curr.property]: curr.message });
          return prev;
        }, {});
        mantineForm.setErrors(formErrors);
      }
    } catch (error) {
      showNotification({ message: "Error updating park", color: "red" });
    }
  };

  return (
    <Container>
      {park && (
        <form onSubmit={mantineForm.onSubmit(submitPark)}>
          <TextInput
            {...mantineForm.getInputProps("name")}
            label="Name"
            withAsterisk
          />
          <Space h={18} />
          <Flex direction={"row"}>
            <Button type="submit">Submit</Button>
            <Space w={8} />
            <Button
              type="button"
              onClick={() => {
                navigate(routes.ParksListing);
              }}
              variant="outline"
            >
              Cancel
            </Button>
          </Flex>
        </form>
      )}
    </Container>
  );
};