import React, { useEffect, useState } from "react";
import { Button, Container, Flex, Space, TextInput } from "@mantine/core";
import { FormErrors, useForm } from "@mantine/form";
import { useNavigate, useParams } from "react-router-dom";
import { showNotification } from "@mantine/notifications";
import { routes } from "../../routes";
import { ParksCreateUpdateDto, ParksGetDto } from "../../constants/types";
import { db } from "../../config/dapper"; // Assuming you have a Dapper database instance named 'db'

export const GameConsoleUpdate = () => {
    const [park, setPark] = useState<ParksGetDto | undefined>(undefined);

  const navigate = useNavigate();
  const { id } = useParams();

  const mantineForm = useForm<ParksCreateUpdateDto>({
    initialValues: park
  });

  useEffect(() => {
  fetchPark(); // Remove the extra curly braces here

  async function fetchPark() {
    const response = await db.get<ParksGetDto>(
      `/api/park/${id}`
    );

    if (response.hasErrors) {
      showNotification({ message: "Error finding park", color: "red" });
    }

    if (response.data) {
      setPark(response.data);
      mantineForm.setValues(response.data);
      mantineForm.resetDirty();
    }
  }
}, [id]);


  const submitPark = async (values) => {
    const response = await db.put<ParksGetDto>(
      `/api/park/${id}`,
      values
    );

    if (response.hasErrors) {
      const formErrors = response.errors.reduce((prev, curr) => {
        Object.assign(prev, { [curr.property]: curr.message });
        return prev;
      }, {} );
      mantineForm.setErrors(formErrors);
    }

    if (response.data) {
      showNotification({
        message: "Park successfully updated",
        color: "green"
      });
      navigate(routes.ParksListing);
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